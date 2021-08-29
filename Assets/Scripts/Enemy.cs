using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 50f;
    public int score = 50;
    public int touchDamage = 10;

    public GameObject bullet;
    private GameObject WaveSpawner;
    private GameObject player;
    private Rigidbody2D rb;

    public float fireRate = 1f;
    private float fireRateCooldown = 0f;
    public float firstShotDelay = 1f;

    public bool launched = false;
    private bool isDead = false;
    public float launchSpeed = 10f;

    private void Start()
    {
        WaveSpawner = GameObject.Find("WaveSpawner");
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && fireRate >= 0)
        {
            //Fire a bullet every "fireRate" seconds
            if (firstShotDelay <= 0)
            {
                if (Time.time >= fireRateCooldown)
                {
                    Fire(0f);
                }
            }
            else
                firstShotDelay -= Time.deltaTime;
        }

        if (rb.velocity == Vector2.zero)
            launched = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDead && collision.gameObject.CompareTag("PlayerBullet"))
        {
            try
            {
                //Debug.Log("Player bullet hit Enemy!");
                isDead = true;
                WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(score);
                StartCoroutine(Flash(gameObject.GetComponent<SpriteRenderer>(), 1, 0.1f));
                Destroy(gameObject, 1.2f);
                //insert death animation here
            }
            catch (MissingReferenceException)
            {
                Debug.Log("Missing Reference Exception: Enemy already dead");
            }
        }
        if(!isDead && launched && collision.CompareTag("Enemy"))
        {
            try
            {
                isDead = true;
                StartCoroutine(Flash(gameObject.GetComponent<SpriteRenderer>(), 1, 0.1f));
                Destroy(gameObject, 1.2f);
                WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(score);

                collision.gameObject.GetComponent<Enemy>().launched = true;
                collision.gameObject.GetComponent<Enemy>().isDead = true;
                StartCoroutine(Flash(collision.gameObject.GetComponent<SpriteRenderer>(), 1, 0.1f));
                Destroy(collision.gameObject, 1.2f);
                WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(collision.GetComponent<Enemy>().score);
            }
            catch (MissingReferenceException)
            {
                Debug.Log("Missing Reference Exception: Enemy already dead");
            }
        }
        if(!launched && collision.gameObject.CompareTag("Reflect"))
        {
            Debug.Log("Launched Enemy!");
            Launch(player.GetComponent<PlayerReflect>().getLastMousePositionWithOffset(), launchSpeed);
        }
    }

    private void Fire(float angle)
    {
        bullet.GetComponent<NormalBullet>().setAngleOffset(angle);
        Instantiate(bullet, transform.position, transform.rotation);
        fireRateCooldown = Time.time + fireRate;
    }

    public void Launch(Vector3 direction, float speed)
    {
        launched = true;
        rb.velocity = ((direction - transform.position).normalized * speed);
    }

    IEnumerator Flash(SpriteRenderer sprite, float duration, float rate)
    {
        for (int n = 0; n < duration / rate / 2; n++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(rate);
            sprite.color = Color.white;
            yield return new WaitForSeconds(rate);
        }
    }

}
