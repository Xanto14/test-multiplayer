using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreDisplay;
    [SerializeField] private GameObject multiplierIcon;
    [SerializeField] private GameObject speedOverlay;
    [SerializeField] private GameObject inkOverlay;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] public GameObject playerGameObject;
    [SerializeField] private GameObject vortexPrefab;


    [SerializeField] private GameObject explosionPrefab;

    

    private TileController tileController;
    private bool isPlaying;
    private float playerScoreFloat;
    private int playerScoreInt;
    private int lastScoreSlice;
    public int scoreSliceSize;
    private float scoreMultiplierOvertime;
    private int deathZoneHeight = -5;
    private Transform playerTransform;
    public bool collidedWithEnemy;
    private int difficulty;
    private Color wallColor;

    public int Difficulty { get { return Difficulty; } }

    public bool IsPlaying { get => isPlaying; }

    public GameObject MultiplierIcon { get => multiplierIcon; }

    public GameObject SpeedOverlay { get => speedOverlay; }


    private void Awake()
    {
        tileController = gameObject.GetComponent<TileController>();
        isPlaying = false;
        collidedWithEnemy = false;
        lastScoreSlice = 0;
        scoreSliceSize = 5000;
        scoreMultiplierOvertime = 110;
        gameOverDisplay.SetActive(false);
        playerTransform = playerGameObject.transform;
    }

    private void Update()
    {
        UpdateScore();
        ManageDifficulty();
        GameOverCondition();
    }


    private void ManageDifficulty()
    {
        int currentScoreSlice = Mathf.FloorToInt(playerScoreInt / scoreSliceSize);
        if (currentScoreSlice > lastScoreSlice)
        {
            difficulty++;
            scoreMultiplierOvertime *= 1.025f;
            tileController.SetMaxIterations(difficulty);
            wallColor = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
            foreach (var wallObstacle in tileController.spawnedWalls)
            {
                if (wallObstacle != null)
                {
                    Renderer renderer = wallObstacle.GetComponentInChildren<Renderer>();

                    if (renderer != null)
                        renderer.material.color = wallColor;
                }
            }

            vortexPrefab.GetComponent<EnemyManager>().moveSpeed += 25 * (scoreSliceSize / 10000f);

            lastScoreSlice = currentScoreSlice;
        }
    }


    private void UpdateScore()
    {
        if (isPlaying)
        {
            playerScoreFloat += Time.deltaTime * scoreMultiplierOvertime *
                                playerGameObject.GetComponent<HoverMotor>().scoreMultiplier;
            playerScoreInt = (int) playerScoreFloat;

            //scoreDisplay.text = $"{playerScore:f0}";
            scoreDisplay.text = playerScoreInt.ToString();
        }
    }

    private void GameOverCondition()
    {
        if (playerTransform.position.y < deathZoneHeight)
        {
            if (!gameOverDisplay.activeSelf)
            {
                StartCoroutine(GameOverSequence(playerGameObject));
                GameOverSequence(playerGameObject);
            }
        }

        if (collidedWithEnemy)
        {
            if (!gameOverDisplay.activeSelf)
            {
                StartCoroutine(GameOverSequence(playerGameObject));
                GameOverSequence(playerGameObject);
            }
        }
    }

    IEnumerator GameOverSequence(GameObject player)
    {
        isPlaying = false;

        if (player.transform.GetChild(2).gameObject.activeSelf)
        {
            Instantiate(explosionPrefab, player.transform.GetChild(2).gameObject.transform.position,
                Quaternion.identity);
            player.transform.GetChild(2).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        gameOverDisplay.SetActive(true);
    }

    public void GameStartSequence()=> isPlaying = true;
    public void ApplyInk(bool status) => inkOverlay.SetActive(status);

    public void OnClickLoadScene(int scene)
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
        
        SceneManager.LoadScene(scene);
    }

    public void RemoveScore(int score)
    {
        if(playerScoreFloat>1000)
        playerScoreFloat -= score;
    }

}