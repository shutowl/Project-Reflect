using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public int enemiesLeft = 4;
    public float timePerEnemy = 5f;
    public float maxTime = 30f;
    public Text timerText;
    public Text enemiesLeftText;
    public Text scoreText;
    public Text waveText;
    public Text multiplierText;

    private float waveTimer = 0f;
    private bool waveComplete = false;
    private int enemyWaveOffset = 0;
    private int waveNumber = 1;

    public Vector2 boundsMin;
    public Vector2 boundsMax;

    public GameObject[] enemies;

    private int score = 0;
    public int waveScore = 100;
    public int timeScore = 50;
    private float multiplier = 1.0f;

    private void Start()
    {
        waveTimer = timePerEnemy * enemiesLeft;
        timerText.text = "Next Wave: " + System.Math.Round(waveTimer, 2);
        enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        scoreText.text = "Score: " + score;
        waveText.text = "Wave: " + waveNumber;
        multiplierText.text = "Combo: x" + System.Math.Round(multiplier, 1);
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer -= Time.deltaTime;
        timerText.text = "Next Wave: " + System.Math.Round(waveTimer, 2);

        if(waveComplete)
        {
            score += timeScore * (int)waveTimer * (int)multiplier;
            waveTimer = 5f;
            waveComplete = false;
            score += waveScore * waveNumber * (int)multiplier;
            scoreText.text = "Score: " + score;
        }

        if(waveTimer <= 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        //Set wave number
        waveNumber++;
        waveText.text = "Wave: " + waveNumber;

        //Spawn random number of enemies (Ex: between 3-6)
        //random offset increases every wave (Ex: from 3-6 to 4-7)
        if (waveNumber >= 0 && waveNumber <= 10)
        {
            enemyWaveOffset++;
            int addEnemies = Random.Range(3 + enemyWaveOffset, 6 + enemyWaveOffset);
            enemiesLeft += addEnemies;
            SpawnSquare(addEnemies);
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        }
        //Boss battle at wave 10
        if(waveNumber == 10)
        {
            enemyWaveOffset = 0;
        }
        if(waveNumber > 10 && waveNumber <= 20)
        {
            enemyWaveOffset++;
            int addEnemies = Random.Range(3 + enemyWaveOffset, 6 + enemyWaveOffset);
            enemiesLeft += addEnemies;
            //Spawn different enemies
            SpawnSquare(addEnemies);
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        }

        //Reset wave time (depends on number of enemies)
        if (timePerEnemy * enemiesLeft >= maxTime)
            waveTimer = maxTime;
        else
            waveTimer = timePerEnemy * enemiesLeft;
    }

    private void SpawnSquare(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Instantiate(enemies[0], new Vector3(Random.Range(boundsMin.x, boundsMax.x), Random.Range(boundsMin.y, boundsMax.y), 0), Quaternion.identity);
        }
    }

    public void enemyKilled(int score)
    {
        //set multiplier text
        if (multiplier < 5.0f)
        {
            multiplier += 0.1f;
            multiplierText.text = "Combo: x" + System.Math.Round(multiplier, 1);
        }

        //set enemies killed text
        if (enemiesLeft > 0)
        {
            enemiesLeft--;
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        }

        //set score text
        this.score += (int)(score * multiplier);
        scoreText.text = "Score: " + this.score;


        if(enemiesLeft <= 0)
        {
            waveComplete = true;
        }
    }

    public void resetMultiplier()
    {
        //set multiplier text
        multiplier = 1.0f;
        multiplierText.text = "Combo: x" + System.Math.Round(multiplier, 1);
    }


}
