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
    public bool isDead = false;
    public float launchSpeed = 10f;

    private void Start()
    {
        launched = false;
        isDead = false;
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

        if (rb.velocity.x <= 0.5 && rb.velocity.y <= 0.5)
        {
            rb.velocity = Vector2.zero;
            launched = false;
        }
        else
            launched = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDead && collision.gameObject.CompareTag("PlayerBullet"))
        {
            Death(this.gameObject);
        }
        if(launched && collision.CompareTag("Enemy"))
        {
            if (!isDead)
            {
                Death(this.gameObject);
            }
            if (!collision.gameObject.GetComponent<Enemy>().isDead)
            {
                Death(collision.gameObject);
            }

        }
        if (!launched && collision.gameObject.CompareTag("Reflect"))
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

    private void Death(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().isDead = true;
        StartCoroutine(Flash(enemy.gameObject.GetComponent<SpriteRenderer>(), 1, 0.1f));
        Destroy(enemy.gameObject, 1f);
        WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(enemy.GetComponent<Enemy>().score);
        //insert death animation here
    }

    IEnumerator Flash(SpriteRenderer sprite, float duration, float rate)
    {
        for (int n = 0; n < duration / rate / 2; n++)
        {
            if (sprite != null)
            {
                sprite.color = Color.red;
                yield return new WaitForSeconds(rate);
                sprite.color = Color.white;
                yield return new WaitForSeconds(rate);
            }
        }
    }


}
