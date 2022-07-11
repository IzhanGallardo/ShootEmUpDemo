using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator generator = null;

    static EnemyPool enemyPool;

    public float[] alienStage1Interval = new float[2];
    public float[] alienStage2Interval = new float[2];
    public float[] alienStage3Interval = new float[2];
    float[] alienInterval = new float[2];
    public float nextAlien;
    public float alienTimer = 0f;

    public float[] asteroidStage1Interval = new float[2];
    public float[] asteroidStage2Interval = new float[2];
    public float[] asteroidStage3Interval = new float[2];
    float[] asteroidInterval = new float[2];
    public float nextAsteroid;
    public float asteroidTimer = 0f;

    Scene curScene;

    private void Awake()
    {
        if (generator == null)
        {
            generator = this;
        } else if (generator != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        manageTimeIntervals();

        curScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Stage1" || SceneManager.GetActiveScene().name == "Stage2" || SceneManager.GetActiveScene().name == "Stage3")
        { 
            alienTimer += Time.deltaTime;
            asteroidTimer += Time.deltaTime;

            if (alienTimer >= nextAlien)
            {
                generateAlien();
                alienTimer = 0f;
                nextAlien = Random.Range(alienInterval[0], alienInterval[1]);
            }

            if (asteroidTimer >= nextAsteroid)
            {
                generateAsteroid();
                asteroidTimer = 0f;
                nextAsteroid = Random.Range(asteroidInterval[0], asteroidInterval[1]);
            }
        } else
        {
            Destroy(gameObject);
        }

        if(SceneManager.GetActiveScene() != curScene)
        {
            curScene = SceneManager.GetActiveScene();
            manageTimeIntervals();
        }
    }

    void generateAlien()
    {
        float randomAlienY = Random.Range(-4.5f, 8f);

        enemyPool.generateAlien(new Vector3(16.1f, randomAlienY, 0));
    }

    void generateAsteroid()
    {
        float randomAsteroidY = Random.Range(-4.5f, 8f);

        enemyPool.generateAsteroid(new Vector3(16.1f, randomAsteroidY, 0));
    }

    void manageTimeIntervals()
    {
        if (SpaceshipController.stage == 1)
        {
            alienInterval[0] = alienStage1Interval[0];
            alienInterval[1] = alienStage1Interval[1];

            asteroidInterval[0] = asteroidStage1Interval[0];
            asteroidInterval[1] = asteroidStage1Interval[1];
        }
        else if (SpaceshipController.stage == 2)
        {
            alienInterval[0] = alienStage2Interval[0];
            alienInterval[1] = alienStage2Interval[1];

            asteroidInterval[0] = asteroidStage2Interval[0];
            asteroidInterval[1] = asteroidStage2Interval[1];
        }
        else if (SpaceshipController.stage == 3)
        {
            alienInterval[0] = alienStage3Interval[0];
            alienInterval[1] = alienStage3Interval[1];

            asteroidInterval[0] = asteroidStage3Interval[0];
            asteroidInterval[1] = asteroidStage3Interval[1];
        }

        nextAlien = Random.Range(alienInterval[0], alienInterval[1]);
        nextAsteroid = Random.Range(asteroidInterval[0], asteroidInterval[1]);
    }

    public static void searchForPool()
    {
        enemyPool = FindObjectOfType<EnemyPool>();
    }

}
