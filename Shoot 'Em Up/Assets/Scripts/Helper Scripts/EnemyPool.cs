using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public int alienPoolSize = 10;
    public GameObject[] alien = new GameObject[3];
    private GameObject[] aliens;
    public int alienNumber = -1;

    public int asteroidPoolSize = 10;
    public GameObject[] asteroid = new GameObject[6];
    private GameObject[] asteroids;
    public int asteroidNumber = -1;

    // Start is called before the first frame update
    void Start()
    {
        aliens = new GameObject[alienPoolSize];
        for (int i = 0; i < alienPoolSize; i++)
        {
            aliens[i] = Instantiate(alien[Random.Range(0, 3)], new Vector3(-25f, -25f, 0f), transform.rotation * Quaternion.Euler(0f, 0f, 90f));
        }

        asteroids = new GameObject[asteroidPoolSize];
        for (int i = 0; i < asteroidPoolSize; i++)
        {
            asteroids[i] = Instantiate(asteroid[Random.Range(0, 6)], new Vector3(-25f, -25f, 0f), Quaternion.identity);
        }

        EnemyGenerator.searchForPool();
    }

    public void generateAlien(Vector3 pos)
    {
        alienNumber++;
        if (alienNumber > alienPoolSize - 1)
        {
            alienNumber = 0;
        }

        aliens[alienNumber].transform.position = pos;
        aliens[alienNumber].SetActive(true);
    }

    public void generateAsteroid(Vector3 pos)
    {
        asteroidNumber++;
        if (asteroidNumber > asteroidPoolSize - 1)
        {
            asteroidNumber = 0;
        }

        asteroids[asteroidNumber].transform.position = pos;
        asteroids[asteroidNumber].SetActive(true);
    }
}
