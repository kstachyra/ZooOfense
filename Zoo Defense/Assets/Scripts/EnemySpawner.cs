using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public AnimationCurve wave;
    public int roundTime;
    public int enemies;
    public int enemyTypes;

    int toSpawn;
    int spawned;

    float timeFrame;
    float startTime;

    int level = 1;

    public void StartNewLevel()
    {
        //TODO
        toSpawn = enemies;
        spawned = 0;


        startTime = Time.time;
        timeFrame = roundTime / 10;

        StartCoroutine("SpawnEnemies");
    }

    public void EndLevel()
    {
        //TODO
        level += 1;

        UpdateUI();

        StartNewLevel();
    }

    private void Start()
    {
        //!!! TODO
        level = 1;
        StartNewLevel();
    }

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            float timePassed = (Time.time - startTime);
            float timeLeft = roundTime - timePassed;
            int howMany = (int)(((toSpawn - spawned) / timeLeft) * timeFrame);
            howMany = (int)(howMany * UnityEngine.Random.Range(0.5f, 1.5f) * wave.Evaluate(timePassed / roundTime)) * level;
            howMany++;
            spawned += howMany;

            for(int i = 0; i < howMany; ++i)
            {
                int enemyType = 0;

                //upgrade enemy
                for(int e = 0; e < enemyTypes -1; ++e)
                {
                    if(UnityEngine.Random.Range(0.0f, 2f) < wave.Evaluate(timePassed / roundTime))
                    {
                        enemyType++;
                    }
                }
                StartCoroutine("SpawnEnemy", enemyType);
            }

            yield return new WaitForSeconds(timeFrame);

            if(toSpawn - spawned <= 0)
            {
                EndLevel();
                yield break;
            }
        }
    }

    private IEnumerator SpawnEnemy(int enemyType)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, timeFrame));

        Debug.Log("Spawned: enemy " + enemyType);

        Enemy newEnemy;
        switch (enemyType)
        {
            case 0:
                newEnemy = (Instantiate(Enemy1)).GetComponent<Enemy>();
                break;
            case 1:
                newEnemy = (Instantiate(Enemy2)).GetComponent<Enemy>();
                break;
            case 2:
                newEnemy = (Instantiate(Enemy3)).GetComponent<Enemy>();
                break;
            default:
                newEnemy = (Instantiate(Enemy1)).GetComponent<Enemy>();
                break;
        }

        //w poziomie
        int x = GameManager.instance.ySize;
        //w pionie
        int y = GameManager.instance.xSize;

        y = Random.Range(0, y);
        Node enemyPosition = new Node(x-1, y);

        newEnemy.transform.position = new Vector3(enemyPosition.X, enemyPosition.Y, transform.position.z);
        newEnemy.CalculatePath(enemyPosition, GameManager.destination);

        yield break;
    }

    private void UpdateUI()
    {
        UIManager.instance.SetLevel(level);
    }
}