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

    public GameObject ReflectBlack;
    public GameObject ReflectRed;

    public float reflectRateBlack = 1f;
    private float reflectCooldownBlack = 0f;
    private bool canReflectBlack = true;

    public float reflectRateRed = 1f;
    private float reflectCooldownRed = 0f;
    private bool canReflectRed = true;

    private Vector3 lastMousePos = new Vector3();
    private Vector3 mousePos = new Vector3();
    private Vector3 posOffsetVector = new Vector3();
    private float mouseAngle = 0f;

    public Slider reflectSliderBlack;
    public Image fillBlack;
    public Slider reflectSliderRed;
    public Image fillRed;

    public float reflectDelay = 0.5f; //delay between reflects
    private float reflectDelayTimer = 0f;

    public bool laserPointer = true;
    private LineRenderer laser;


    private void Start()
    {
        reflectSliderBlack.maxValue = reflectRateBlack;
        reflectSliderBlack.value = reflectRateBlack;
        reflectSliderRed.maxValue = reflectRateRed;
        reflectSliderRed.value = reflectRateRed;

        laser = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse Position: " + mousePos);

        //Fire a reflect every "reflectRate" seconds
        if (!canReflectBlack && Time.time >= reflectCooldownBlack)
        {
            canReflectBlack = true;
        }
        if (!canReflectRed && Time.time >= reflectCooldownRed)
        {
            canReflectRed = true;
        }

        // reflect is available, left click = reflect
        if (reflectDelayTimer <= 0 && canReflectBlack && Input.GetMouseButton(0))
        {
            reflectRasetsu();
            reflectSliderBlack.value = 0f;
        }
        if(reflectDelayTimer <= 0 && canReflectRed && Input.GetMouseButton(0))
        {
            reflectAshura();
            reflectSliderRed.value = 0f;
        }

        // Slider code
        if (!canReflectBlack)
        {
            reflectSliderBlack.gameObject.SetActive(true);
            reflectSliderBlack.value += Time.deltaTime;
        }
        else
            reflectSliderBlack.gameObject.SetActive(false);

        if (!canReflectRed)
        {
            reflectSliderRed.gameObject.SetActive(true);
            reflectSliderRed.value += Time.deltaTime;
        }
        else
            reflectSliderRed.gameObject.SetActive(false);

        if(reflectDelayTimer >= 0)
        {
            reflectDelayTimer -= Time.deltaTime;
        }

        laser.enabled = laserPointer;
        if (laserPointer)
        {
            Vector3[] pos = new Vector3[2];
            pos[0] = transform.position;
            pos[1] = mousePos;
            laser.startWidth = 0.03f;
            laser.endWidth = 0.03f;
            laser.startColor = Color.red;
            laser.endColor = Color.red;
            laser.SetPositions(pos);
            laser.useWorldSpace = true;
        }

    }

    private void FixedUpdate()
    {
        //Mouse angle in Converted from RAD to DEG
        mouseAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log("Mouse Angle:" + mouseAngle);
    }

    //summons reflect at player position + offset
    //offset prevents reflect from summoning behind the player
    private void reflectRasetsu()   //black sword
    {
        canReflectBlack = false;
        reflectDelayTimer = reflectDelay;

        lastMousePos = mousePos;

        posOffsetVector.x = positionOffset * Mathf.Cos(mouseAngle * Mathf.Deg2Rad);
        posOffsetVector.y = positionOffset * Mathf.Sin(mouseAngle * Mathf.Deg2Rad);
        Instantiate(ReflectBlack, transform.position + posOffsetVector, Quaternion.identity);

        reflectCooldownBlack = Time.time + reflectRateBlack;
    }

    private void reflectAshura()    //red sword
    {
        canReflectRed = false;
        reflectDelayTimer = reflectDelay;

        lastMousePos = mousePos;

        posOffsetVector.x = positionOffset * Mathf.Cos(mouseAngle * Mathf.Deg2Rad);
        posOffsetVector.y = positionOffset * Mathf.Sin(mouseAngle * Mathf.Deg2Rad);
        Instantiate(ReflectRed, transform.position + posOffsetVector, Quaternion.identity);

        reflectCooldownRed = Time.time + reflectRateRed;
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

    public Vector3 getLastMousePositionWithOffset()
    {
        Vector3 offset = new Vector3();
        offset.x = 3f * Mathf.Cos(getMouseAngle() * Mathf.Deg2Rad);
        offset.y = 3f * Mathf.Sin(getMouseAngle() * Mathf.Deg2Rad);

        return lastMousePos + offset;
    }
}
