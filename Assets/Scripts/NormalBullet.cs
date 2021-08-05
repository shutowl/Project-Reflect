using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float speed = 4f;

    public GameObject target;
    Rigidbody2D rb;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        
        //Determine direction of bullet
        direction = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(direction.x, direction.y);

        //Rotate bullet towards player
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan((transform.position.y - target.transform.position.y)/(transform.position.x - target.transform.position.x)) * 180 / Mathf.PI);
        
        //Destroy bullet after x seconds
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Debug.Log("Bullet Hit Player!");
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(10);
            Destroy(gameObject);
        }
        if (collision.gameObject.name.Equals("TriggerWall"))
        {
            //Debug.Log("Bullet Hit Wall!");
            Destroy(gameObject);
        }
    }
}
