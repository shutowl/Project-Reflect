using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalBullet : MonoBehaviour
{
    public float speed = 4f;
    public float offset = 3f;
    public float angleOffset = 90f;
    public float explosionLength = 0.4f;

    private Vector3 mousePos = new Vector3();
    private float mouseAngle = 0f;
    private Vector3 mouseOffset = new Vector3();
    Rigidbody2D rb;
    public Animator animator;

    private Vector2 direction;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        speed = GameObject.FindGameObjectWithTag("Reflect").GetComponent<Reflect>().getBulletSpeed();
        player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>();

        //Determine mouse position and angle
        mousePos = player.GetComponent<PlayerReflect>().getLastMousePosition();

        mouseOffset.x = offset * Mathf.Cos(player.GetComponent<PlayerReflect>().getMouseAngle() * Mathf.Deg2Rad);
        mouseOffset.y = offset * Mathf.Sin(player.GetComponent<PlayerReflect>().getMouseAngle() * Mathf.Deg2Rad);

        mousePos += mouseOffset;
        mouseAngle = player.GetComponent<PlayerReflect>().getMouseAngle(transform.position, mousePos);

        //Determine direction of bullet
        direction = (mousePos - transform.position).normalized * speed;
        rb.velocity = new Vector2(direction.x, direction.y);

        //Rotate bullet towards last mouse position
        transform.localEulerAngles = new Vector3(0, 0, mouseAngle + angleOffset);

        //Destroy bullet after x seconds
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("TriggerWall"))
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("collision");
            Destroy(gameObject, explosionLength);
        }
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("collision");
            Destroy(gameObject, explosionLength);
        }

    }
}
