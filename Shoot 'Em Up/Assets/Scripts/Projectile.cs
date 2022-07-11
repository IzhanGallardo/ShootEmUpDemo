using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public GameObject hit_effect;
	public GameObject firing_ship;

    public float prjSpeed = 12f;
    public float lifeTime = 1f;
    float lifeTimer;
	// Use this for initialization
	void Start () 
    {
        lifeTimer = lifeTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(lifeTimer > 0f)
        {
            lifeTimer -= Time.deltaTime;
            transform.position += new Vector3(prjSpeed * Time.deltaTime, 0, 0);
        }

        if(lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(-20, -20, 0);
        }


        if(firing_ship == null)
        {
            gameObject.transform.position = new Vector3(-20, -20, 0);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if(firing_ship != null)
        {
            if (firing_ship.tag == "Enemy")
            {
                if (col.gameObject.tag == "Player")
                {
                    Instantiate(hit_effect, transform.position, Quaternion.identity);

                    gameObject.SetActive(false);
                    gameObject.transform.position = new Vector3(-20, -20, 0);
                }
            }
            else if (firing_ship.tag == "Player")
            {
                if (col.gameObject.tag == "Enemy")
                {
                    Instantiate(hit_effect, transform.position, Quaternion.identity);

                    gameObject.SetActive(false);
                    gameObject.transform.position = new Vector3(-20, -20, 0);
                }
            }
        }        
	}

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(-20, -20, 0);
    }

    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }
}
