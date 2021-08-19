using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public float health = 50f;
    public int score = 50;
    public int touchDamage = 10;

    public GameObject bullet;
    private GameObject WaveSpawner;
    private Rigidbody2D rb;

    public float fireRate = 1f;
    private float fireRateCooldown = 0f;
    public float firstShotDelay = 1f;

    public float deceleration = 0.6f;
    private bool launched = false;
    private bool onWall = false;

    private void Start()
    {
        WaveSpawner = GameObject.Find("WaveSpawner");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate >= 0)
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

        if (launched && rb.velocity != Vector2.zero)
        {
            //rb.velocity = rb.velocity * deceleration;
        }
        if (rb.velocity == Vector2.zero)
            launched = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            //Debug.Log("Player bullet hit Enemy!");
            WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(score);
            Destroy(gameObject);
        }
        if(launched && collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(score);
            Destroy(collision.gameObject);
            WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(collision.GetComponent<Square>().score);
        }
        if (launched && collision.CompareTag("Wall"))
        {
            onWall = true;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (launched && collision.CompareTag("Wall"))
        {
            onWall = false;
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
        if (!onWall)
        {
            launched = true;
            rb.velocity = ((direction - transform.position).normalized * speed);
        }
    }
}
