using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public UnityEngine.Object Enemy;

    public AnimationCurve wave;
    public int roundTime;
    public int enemies;
    public int enemyTypes;

    int toSpawn;
    int spawned;

    float timeFrame;
    float startTime;

    int level = 0;

    public void startNewLevel()
    {
        //TODO
        toSpawn = enemies;
        spawned = 0;


        startTime = Time.time;
        timeFrame = roundTime / 10;

        StartCoroutine("SpawnEnemies");
    }

    public void endLevel()
    {
        //TODO
        level += 1;

        UpdateUI();

        startNewLevel();
    }

    private void Start()
    {
        //!!! TODO
        level = 0;
        startNewLevel();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float timePassed = (Time.time - startTime);
            float timeLeft = roundTime - timePassed;
            int howMany = (int)(((toSpawn - spawned) / timeLeft) * timeFrame);
            howMany = (int)(howMany * UnityEngine.Random.Range(0.5f, 1.5f) * wave.Evaluate(timePassed / roundTime));
            howMany++;
            spawned += howMany;

            for (int i = 0; i < howMany; ++i)
            {
                int enemyType = 0;

                //upgrade enemy
                for (int e = 0; e < enemyTypes; ++e)
                {
                    if (UnityEngine.Random.Range(0.0f, 2f) < wave.Evaluate(timePassed / roundTime))
                    {
                        enemyType++;
                    }
                }
                StartCoroutine("SpawnEnemy", enemyType);
            }

            yield return new WaitForSeconds(timeFrame);

            if (toSpawn - spawned <= 0)
            {
                endLevel();
                yield break;
            }
        }
    }

    private IEnumerator SpawnEnemy(int enemyType)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, timeFrame));
        Debug.Log("Spawned: enemy " + enemyType);
        Instantiate(Enemy);
        yield break;
    }

    private void UpdateUI()
    {
        UIManager.instance.SetLevel(level);
    }
}