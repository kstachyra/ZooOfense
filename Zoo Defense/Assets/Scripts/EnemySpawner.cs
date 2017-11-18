using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    int level = 0;

    public void StartNewGame()
    {
        level = 0;
    }

    private void SpawnEnemy()
    {

    }

    private void UpdateUI()
    {
        UIManager.instance.SetLevel(level);
    }
}
