﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public Text levelText;
    public Text gameOverText;
    public float difficultyIncrease = 1.2f;

    private float highestPosition;
    private int score = 0;
    private int level = 1;
    private float restartTimer = 3f;


    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        player.OnPlayerMoved += OnPlayerMoved;
        player.OnPlayerEscaped += OnPlayerEscaped;

        highestPosition = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            gameOverText.gameObject.SetActive(true);

            restartTimer -= Time.deltaTime;
            if(restartTimer <= 0f)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    void OnPlayerMoved()
    {
        if (player.transform.position.y > highestPosition)
        {
            highestPosition = player.transform.position.y;
            score = score + 10;
            scoreText.text = "Score: " + score;
        }
    }

    void OnPlayerEscaped()
    {
        highestPosition = player.transform.position.y;
        level++;
        levelText.text = "Level: " + level;
        score = score + 90;
        scoreText.text = "Score: " + score;
        foreach(Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.speed *= difficultyIncrease;
        }
    }
}
