using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private GameObject playerGameObject;
    
    
    private bool isPlaying;
    private float playerScore;
    private int scoreMultiplier;
    private int deathZoneHeight = -5;
    private Transform playerTransform;
    public bool collidedWithEnemy;
    public bool IsPlaying { get => isPlaying; }

    private void Awake()
    {
        isPlaying = false;
        collidedWithEnemy = false;
        scoreMultiplier = 110;
        gameOverDisplay.SetActive(false);
        playerTransform = playerGameObject.transform;
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
            playerScore += Time.deltaTime * scoreMultiplier;


            scoreDisplay.text = $"{playerScore:f0}";
        }
    }

    private void gameOverCondition()
    {
        if (playerTransform.position.y < deathZoneHeight)
        {
            GameOverSequence(playerGameObject);
        }

        if (collidedWithEnemy)
        {
            GameOverSequence(playerGameObject);
        }
        
    }

    public void GameOverSequence(GameObject player)
    {
        isPlaying = false;
        gameOverDisplay.SetActive(true);
    }
    public void GameStartSequence()
    {
        isPlaying = true;
    }
}
