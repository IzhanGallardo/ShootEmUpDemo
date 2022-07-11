using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShipBehaviour : MonoBehaviour
{
    ProjectilePool projectilePool;

    public float spd = 10f;

    public float fireRate = 0.5f;
    public float nextFire = 0f;
    bool blasted = false;
    float blastedTimer = 0f;

    public GameObject hit_effect;

    private SpaceshipController eventReference;

    private void Awake()
    {
        eventReference = FindObjectOfType<SpaceshipController>();
        projectilePool = FindObjectOfType<ProjectilePool>();
    }

    // Update is called once per frame
    void Update()
    {
        manageBlast();

        if (!blasted)
        {
            move();
            fire();
        }   
    }

    void move()
    {
        transform.position -= new Vector3(spd * Time.deltaTime, 0, 0);
    }

    void fire()
    {
        if (Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            projectilePool.AlienShootProjectile(new Vector3(transform.position.x - 1.3f, transform.position.y, transform.position.z), gameObject, 1f);
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-25, -25, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Projectile" || col.gameObject.tag == "Player")
        {
            Instantiate(hit_effect, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(-25, -25, 0);
        }
    }

    void megablasted()
    {
        Instantiate(hit_effect, transform.position, Quaternion.identity);
        blasted = true;
        blastedTimer = 2f;
    }

    void manageBlast()
    {
        if (blastedTimer > 0f)
        {
            blastedTimer -= Time.deltaTime;
        }

        if (blastedTimer < 0f)
        {
            blastedTimer = 0f;
        }

        if (blastedTimer == 0f)
        {
            blasted = false;
        }
    }

    private void OnEnable()
    {
        eventReference.megaBlastReleased += megablasted;
    }
    private void OnDisable()
    {
        eventReference.megaBlastReleased -= megablasted;
    }
}
