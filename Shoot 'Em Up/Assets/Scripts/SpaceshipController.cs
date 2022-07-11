using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    public static SpaceshipController spaceship = null;

    bool canUp, canDown, canLeft, canRight;

    public float maxSpeed = 10f, curSpeed;

    bool secondaryCharged;
    bool contFire;
    float fireHoldTime;
    const float MINIMUMHELDFIRE = 0.25f;

    public float fireRate = 0.2f;
    float nextFire = 0f;

    public static int maxHP = 3;
    public static int curHP;
    float invTimer = 0f;
    public float invulerability = 1f;

    public static int stage = 1;
    public static float stageTimer;

    public GameObject secondaryAttackParticle;
    static ProjectilePool projectilePool;

    private void Awake()
    {
        if(spaceship == null)
        {
            spaceship = this;
        } else if(spaceship != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        projectilePool = FindObjectOfType<ProjectilePool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canUp = true;
        canDown = true;
        canLeft = true;
        canRight = true;

        secondaryCharged = true;
        contFire = false;
        fireHoldTime = 0f;

        curSpeed = maxSpeed;

        curHP = maxHP;

        manageStage();
    }

    // Update is called once per frame
    void Update()
    {
        applyMovement();

        secondaryAttack();

        primaryAttack();

        manageTimers();
    }

    public delegate void megaBlast();

    public event megaBlast megaBlastReleased;

    void applyMovement()
    {
        if (canRight && Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(curSpeed * Time.deltaTime, 0, 0);
            if (!canLeft)
            {
                canLeft = true;
            }
        }
        if (canLeft && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(curSpeed * Time.deltaTime, 0, 0);
            if (!canRight)
            {
                canRight = true;
            }
        }
        if (canUp && Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, curSpeed * Time.deltaTime, 0);
            if (!canDown)
            {
                canDown = true;
            }
        }
        if (canDown && Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, curSpeed * Time.deltaTime, 0);
            if (!canUp)
            {
                canUp = true;
            }
        }
    }

    void secondaryAttack()
    {
        if (secondaryCharged && Input.GetKeyDown(KeyCode.X))
        {
            secondaryCharged = false;
            Instantiate(secondaryAttackParticle, transform.position + new Vector3(1.3f, 0, 0), Quaternion.identity);

            //call the delegate event so each enemy acts like they are suppoused to
            megaBlastReleased();
        }
        else if (!secondaryCharged && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("No Pum");
        }
    }

    void primaryAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fireHoldTime = Time.timeSinceLevelLoad;
            contFire = false;
            nextFire = 0f;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            if (!contFire)
            {
                projectilePool.SpaceshipShootProjectile(transform.position + new Vector3(1.3f, 0, 0), gameObject, 1.5f);
            }
            contFire = false;
            nextFire = 0f;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (Time.timeSinceLevelLoad - fireHoldTime > MINIMUMHELDFIRE)
            {
                contFire = true;
            }
        }
        if (contFire)
        {
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + fireRate;
                projectilePool.SpaceshipShootProjectile(transform.position + new Vector3(1.3f, 0, 0), gameObject, 1.5f);
            }
        }
    }

    void manageTimers()
    {
        if (invTimer > 0f)
        {
            invTimer -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().enabled = Mathf.PingPong(Time.time, 0.1f) > (0.1f / 2f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (invTimer < 0f)
        {
            invTimer = 0f;
        }

        stageTimer -= Time.deltaTime;

        if (stageTimer < 0f)
        {
            stageTimer = 0f;
        }

        if (stageTimer <= 0f)
        {
            nextStage();
        }
    }

    void manageStage()
    {
        if (stage == 1)
        {
            stageTimer = 30f;
        }
        else if (stage == 2)
        {
            stageTimer = 60f;
        }
        else if (stage == 3)
        {
            stageTimer = 120f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "EnemyProjectile" || col.gameObject.tag == "Enemy")
        {
            getHit();
        }

        if (col.gameObject.tag == "TopBorder")
        {
            canUp = false;
        }
        if (col.gameObject.tag == "BotBorder")
        {
            canDown = false;
        }
        if (col.gameObject.tag == "LeftBorder")
        {
            canLeft = false;
        }
        if (col.gameObject.tag == "RightBorder")
        {
            canRight = false;
        }
    }

    void getHit()
    {
        if(invTimer == 0f)
        {
            curHP--;
            if(curHP <= 0)
            {
                StartCoroutine("EndGame");
                SceneManager.LoadScene("DeadScreen");
                Destroy(gameObject);
            }
            invTimer = invulerability;
        }
    }

    IEnumerator EndGame()
    {
        Destroy(gameObject);
        if(!PlayerPrefs.HasKey("maxStage") || PlayerPrefs.GetInt("maxStage") < stage)
        {
            PlayerPrefs.SetInt("maxStage", stage);
        }
        stage = 1;
        manageStage();
        yield return new WaitForSeconds(0.2f);
    }

    void nextStage()
    {
        secondaryCharged = true;
        curHP = maxHP;

        if (stage == 1)
        {
            stage++;
            manageStage();
            SceneManager.LoadScene("Stage2");
        } else if (stage == 2)
        {
            stage++;
            manageStage();
            SceneManager.LoadScene("Stage3");
        } else if (stage == 3)
        {
            StartCoroutine("EndGame");
            SceneManager.LoadScene("WinScreen");

            Destroy(gameObject);
        }
    }

    public static void searchForPool()
    {
        projectilePool = FindObjectOfType<ProjectilePool>();
    }
}
