using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{

    public float destroyDelay = 0f;

    private GameObject player;
    public GameObject playerNormalBullet;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Rotate towards mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, player.GetComponent<PlayerReflect>().getMouseAngleWithOffset()));
        
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destroyDelay);

    }

    void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("NormalBullet"))
        {
            Debug.Log("Normal Bullet Reflected!");
            Destroy(collision.gameObject);

            //Create a player bullet going opposite direction
            Instantiate(playerNormalBullet, collision.transform.position, Quaternion.identity);
        }
    }


}
