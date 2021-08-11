using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float speed = 4f;
    public int damage = 10;
    public float angleOffset = 90f;
    public float explosionLength = 0.4f;

    public GameObject target;
    Rigidbody2D rb;
    public Animator animator;

    private Vector2 direction;
    private bool reflectable = true;

    // Start is called before the first frame update
    void Start()
    {
        reflectable = true;

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        
        //Determine direction of bullet
        direction = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(direction.x, direction.y);

        //Rotate bullet towards player
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg + angleOffset);
        
        //Destroy bullet after x seconds
        Destroy(gameObject, 5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("PlayerHitbox"))
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

    public bool isReflectable()
    {
        return reflectable;
    }

    public void explode()
    {
        reflectable = false;
        animator.SetTrigger("collision");
        rb.velocity = Vector2.zero;
        Destroy(gameObject, explosionLength);
    }
}
