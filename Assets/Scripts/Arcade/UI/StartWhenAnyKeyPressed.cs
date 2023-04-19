using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWhenAnyKeyPressed : MonoBehaviour
{
    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject gameManager;
    private GameEventManager gameEventManager;

    private void Awake()
    {
        gameEventManager = gameManager.GetComponent<GameEventManager>();
    }

    void Update()
    {
        if (Input.anyKey)
        {
            gameEventManager.GameStartSequence();
            gameOverlay.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}