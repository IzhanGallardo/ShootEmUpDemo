using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public int spaceshipProjectilePoolSize = 10;
    public GameObject spaceshipProjectile;
    private GameObject[] spaceshipProjectiles;
    public int spaceshipShootNumber = -1;

    public int alienProjectilePoolSize = 20;
    public GameObject alienProjectile;
    private GameObject[] alienProjectiles;
    public int alienShootNumber = -1;
    // Start is called before the first frame update
    void Start()
    {
        spaceshipProjectiles = new GameObject[spaceshipProjectilePoolSize];
        for(int i=0; i<spaceshipProjectilePoolSize; i++)
        {
            spaceshipProjectiles[i] = Instantiate(spaceshipProjectile, new Vector3(-20f, -20f), transform.rotation * Quaternion.Euler(0f, 0f, -90f));
        }

        alienProjectiles = new GameObject[alienProjectilePoolSize];
        for (int i = 0; i < alienProjectilePoolSize; i++)
        {
            alienProjectiles[i] = Instantiate(alienProjectile, new Vector3(-20f, -20f), transform.rotation * Quaternion.Euler(0f, 0f, 90f));
        }

        SpaceshipController.searchForPool();
    }

    public void SpaceshipShootProjectile(Vector3 pos, GameObject firingShip, float lifeTime)
    {
        spaceshipShootNumber++;

        if(spaceshipShootNumber > spaceshipProjectilePoolSize-1)
        {
            spaceshipShootNumber = 0;
        }

        if(spaceshipProjectiles[spaceshipShootNumber] != null)
        {
            spaceshipProjectiles[spaceshipShootNumber].transform.position = pos;
            spaceshipProjectiles[spaceshipShootNumber].GetComponent<Projectile>().firing_ship = firingShip;
            spaceshipProjectiles[spaceshipShootNumber].GetComponent<Projectile>().lifeTime = lifeTime;
            spaceshipProjectiles[spaceshipShootNumber].SetActive(true);
        }
    }

    public void AlienShootProjectile(Vector3 pos, GameObject firingShip, float lifeTime)
    {
        alienShootNumber++;

        if (alienShootNumber > alienProjectilePoolSize - 1)
        {
            alienShootNumber = 0;
        }

        if(alienProjectiles[alienShootNumber] != null)
        {
            alienProjectiles[alienShootNumber].transform.position = pos;
            alienProjectiles[alienShootNumber].GetComponent<Projectile>().firing_ship = firingShip;
            alienProjectiles[alienShootNumber].GetComponent<Projectile>().lifeTime = lifeTime;
            alienProjectiles[alienShootNumber].SetActive(true);
        }
    }
}
