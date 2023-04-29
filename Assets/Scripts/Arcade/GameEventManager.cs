using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] public GameObject playerGameObject;
    [SerializeField] private GameObject explosionPrefab;
    
    
    private bool isPlaying;
    private float playerScore;
    private int scoreMultiplierOvertime;
    private int deathZoneHeight = -5;
    private Transform playerTransform;
    public bool collidedWithEnemy;
    public bool IsPlaying { get => isPlaying; }
    private HoverMotor hoverMotorPlayer;

    private void Awake()
    {
        isPlaying = false;
        collidedWithEnemy = false;
        scoreMultiplierOvertime = 110;
        gameOverDisplay.SetActive(false);
        playerTransform = playerGameObject.transform;
        hoverMotorPlayer = playerGameObject.GetComponent<HoverMotor>();
    }

    private void Update()
    {
        updateScore();
        gameOverCondition();
    }
    
    private void updateScore()
    {
        if (isPlaying)
        {
            playerScore += Time.deltaTime * scoreMultiplierOvertime * playerGameObject.GetComponent<HoverMotor>().scoreMultiplier;


            scoreDisplay.text = $"{playerScore:f0}";
        }
    }

    private void gameOverCondition()
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
            Debug.Log("amogus");
            Instantiate(explosionPrefab, player.transform.GetChild(2).gameObject.transform.position, Quaternion.identity);
            player.transform.GetChild(2).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        gameOverDisplay.SetActive(true);
        
    }
    public void GameStartSequence()
    {
        isPlaying = true;
    }
    
    public void OnClickLoadScene(int scene)
    {
        if(Time.timeScale==0f)
            Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    
}
