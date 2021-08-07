using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This script only creates the reflect prefab at the player location
//Actual reflect logic in the reflect.cs script
public class PlayerReflect : MonoBehaviour
{

    public float positionOffset = 1.5f;
    public float angleOffset = -90f;

    public GameObject Reflect;
    public float reflectRate = 1f;
    private float reflectCooldown = 0f;
    private bool canReflect = true;

    private Vector3 lastMousePos = new Vector3();
    private Vector3 mousePos = new Vector3();
    private Vector3 posOffsetVector = new Vector3();
    private float mouseAngle = 0f;

    public Slider reflectSlider;
    public Image fill;


    private void Start()
    {
        reflectSlider.maxValue = reflectRate;
        reflectSlider.value = reflectRate;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse Position: " + mousePos);

        //Fire a reflect every "reflectRate" seconds
        if (!canReflect && Time.time >= reflectCooldown)
        {
            canReflect = true;
        }

        // reflect is available, left click = reflect
        if(canReflect && Input.GetMouseButton(0))
        {
            reflect();
            reflectSlider.value = 0f;
        }

        // Slider code
        if (!canReflect)
        {
            reflectSlider.gameObject.SetActive(true);
            reflectSlider.value += Time.deltaTime;
        }
        else
            reflectSlider.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        //Mouse angle in Converted from RAD to DEG
        mouseAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log("Mouse Angle:" + mouseAngle);
    }

    //summons reflect at player position + offset
    //offset prevents reflect from summoning behind the player
    private void reflect()
    {
        canReflect = false;

        lastMousePos = mousePos;

        posOffsetVector.x = positionOffset * Mathf.Cos(mouseAngle * Mathf.Deg2Rad);
        posOffsetVector.y = positionOffset * Mathf.Sin(mouseAngle * Mathf.Deg2Rad);
        Instantiate(Reflect, transform.position + posOffsetVector, Quaternion.identity);

        reflectCooldown = Time.time + reflectRate;
    }

    //Returns mouse angle in DEGREES
    public float getMouseAngleWithOffset()
    {
        return mouseAngle + angleOffset;
    }

    //Returns mouse angle in DEGREES
    public float getMouseAngle()
    {
        return mouseAngle;
    }

    public float getMouseAngle(Vector3 origin, Vector3 target)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
    }

    //Used to determine player bullet direction
    public Vector3 getLastMousePosition()
    {
        return lastMousePos;
    }
}
