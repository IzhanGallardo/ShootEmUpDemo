using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    public float randomSpin;
    public float randomSpeed;

    public GameObject hit_effect;

    private SpaceshipController eventReference;

    private void Awake()
    {
        eventReference = FindObjectOfType<SpaceshipController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        randomSpin = Random.Range(0.05f, 0.5f);
        randomSpeed = Random.Range(2f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position -= new Vector3(randomSpeed * Time.deltaTime, 0, 0);
        gameObject.transform.Rotate(0f, 0f, randomSpin);
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

        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-25, -25, 0);
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
