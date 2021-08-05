using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public float health = 50f;

    public GameObject bullet;

    public float fireRate = 1f;
    private float fireRateCooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        //Fire a bullet every "fireRate" seconds
        if(Time.time >= fireRateCooldown)
        {
            Fire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("PlayerBullet"))
        {
            Debug.Log("Player bullet hit Enemy!");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(10);
            Debug.Log("Player touched Square!");
        }
    }

    private void Fire()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        fireRateCooldown = Time.time + fireRate;
    }
}
