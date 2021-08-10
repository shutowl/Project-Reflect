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

    public float fireRate = 1f;
    private float fireRateCooldown = 0f;
    public float firstShotDelay = 1f;

    private void Start()
    {
        WaveSpawner = GameObject.Find("WaveSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        //Fire a bullet every "fireRate" seconds
        if (Time.time >= fireRateCooldown && firstShotDelay <= 0)
        {
            Fire();
        }
        else
            firstShotDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("PlayerBullet"))
        {
            Debug.Log("Player bullet hit Enemy!");
            WaveSpawner.GetComponent<WaveSpawner>().enemyKilled(score);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(touchDamage);
            Debug.Log("Player touched Square!");
        }
    }

    private void Fire()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        fireRateCooldown = Time.time + fireRate;
    }
}
