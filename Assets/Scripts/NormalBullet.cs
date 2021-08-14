using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float speed = 4f;
    public int damage = 10;
    public float rotationOffset = 90f;
    public float explosionLength = 0.4f;

    public GameObject target;
    Rigidbody2D rb;
    public Animator animator;

    private Vector3 direction;
    private bool active = true;
    private float angleOffset = 30f;

    // Start is called before the first frame update
    void Start()
    {
        active = true;

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        //Determine direction of bullet
        direction = (target.transform.position - transform.position);
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        //Rotate bullet towards direction
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg + rotationOffset);
        
        //Destroy bullet after x seconds
        Destroy(gameObject, 5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("PlayerHitbox") && active)
        {
            //Debug.Log("Bullet Hit Wall!");
            collision.GetComponentInParent<PlayerHealth>().takeDamage(damage);
            explode();
        }
        if(collision.name.Equals("TriggerWall"))
        {
            explode();
        }
    }

    public bool isActive()
    {
        return active;
    }

    public void explode()
    {
        active = false;
        animator.SetTrigger("collision");
        rb.velocity = Vector2.zero;
        Destroy(gameObject, explosionLength);
    }

    public void setAngleOffset(float angle)
    {
        angleOffset = angle;
    }

}
