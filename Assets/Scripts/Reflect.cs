using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{

    public float destroyDelay = 0f;
    private float bulletSpeed = 0;
    private float angle;
    private Vector3 lastMousePos = new Vector3();

    private GameObject player;
    public GameObject playerNormalBullet;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastMousePos = player.GetComponent<PlayerReflect>().getLastMousePositionWithOffset();

        //Rotate towards mouse
        angle = player.GetComponent<PlayerReflect>().getMouseAngleWithOffset();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destroyDelay);

    }

    void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("NormalBullet") && collision.GetComponent<NormalBullet>().isActive())
        {
            Debug.Log("Normal Bullet Reflected!");
            bulletSpeed = collision.gameObject.GetComponent<NormalBullet>().speed * 2;
            Destroy(collision.gameObject);

            //Create a player bullet going opposite direction
            Instantiate(playerNormalBullet, collision.transform.position, Quaternion.identity);
        }
    }

    public float getBulletSpeed()
    {
        return bulletSpeed;
    }

    public float getReflectAngle()
    {
        return angle;
    }

    public Vector3 getLastMousePos()
    {
        return lastMousePos;
    }
}
