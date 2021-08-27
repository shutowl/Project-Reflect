using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public int enemiesLeft = 4;
    public float timePerEnemy = 5f;
    public float maxTime = 30f;
    public float startTime = 5f;
    public float restTime = 5f;
    public Text timerText;
    public Text enemiesLeftText;
    public Text scoreText;
    public Text waveText;
    public Text multiplierText;

    private float waveTimer = 0f;
    private bool waveComplete = false;
    private int enemyWaveOffset = 0;
    private int waveNumber = 0;

    public Vector2 boundsMin;
    public Vector2 boundsMax;

    public GameObject[] enemies;

    private int score = 0;
    private int lastScore = 0;
    public int waveScore = 100;
    public int timeScore = 50;
    private float multiplier = 1.0f;

    private float timeElapsed = 0f;
    public float lerpDuration = 3f;

    private void Start()
    {
        waveTimer = startTime;
        timerText.text = "Next Wave: " + System.Math.Round(waveTimer, 2);
        enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        scoreText.text = "[" + score.ToString("00000000") + "]";
        waveText.text = "Wave: " + waveNumber;
        multiplierText.text = "- x" + multiplier.ToString("f1") + " -";
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer -= Time.deltaTime;
        timerText.text = "Next Wave: " + System.Math.Round(waveTimer, 2);

        if(waveComplete)
        {
            StartCoroutine(AddScore((int)(timeScore * waveTimer)  +        //time bonus
                                         (waveScore * waveNumber)));       //wave bonus

            waveTimer = restTime;
            waveComplete = false;

            scoreText.text = "[" + score.ToString("00000000") + "]";
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
            multiplierText.text = "- x" + multiplier.ToString("f1") + " -";
        }
        else
        {
            multiplier = 5.0f;
            multiplierText.text = "- x" + multiplier.ToString("f1") + " -";
        }

        //set enemies killed text
        if (enemiesLeft > 0)
        {
            enemiesLeft--;
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
        }

        //set score text
        StartCoroutine(AddScore(score));
        //scoreText.text = "[" + this.score.ToString("00000000") + "]";


        if (enemiesLeft <= 0)
        {
            waveComplete = true;
        }
    }

    public void resetMultiplier()
    {
        //set multiplier text
        multiplier = multiplier / 2;
        if (multiplier <= 1.0f)
            multiplier = 1.0f;
        multiplierText.text = "- x" + multiplier.ToString("f1") + " -";
    }

    IEnumerator AddScore(int score)
    {
        lastScore = this.score;
        timeElapsed = 0f;
        this.score += (int)(score * multiplier);

        while (lastScore != this.score)
        {
            if (timeElapsed < lerpDuration)
            {
                lastScore = Mathf.RoundToInt(Mathf.Lerp(lastScore, this.score, timeElapsed / lerpDuration));
                timeElapsed += Time.deltaTime;
                scoreText.text = "[" + lastScore.ToString("00000000") + "]";
                Debug.Log("Lerp: " + lastScore);
                yield return null;
            }
            else
                lastScore = this.score;
        }
    }


}
