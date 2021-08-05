using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool canMove = true;

    Vector3 lastPosition = new Vector3();
    Vector3 currentDirection = new Vector3();

    public float dashRate = 1f;
    public float dashRecovery = 0.3f;
    public float dashDistance = 3f;
    private float dashCooldown = 0f;
    private float dashRecoveryCooldown = 0f;
    private float unitsFromWall;
    private bool canDash = true;
    private bool facingWall = false;

    public Rigidbody2D rb;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        //Checks how close player is to the wall
        RaycastHit2D rc = Physics2D.Raycast(transform.position+currentDirection, currentDirection, 10f);
        if (rc.collider != null && rc.collider.gameObject.tag.Equals("Wall"))
        {
            unitsFromWall = rc.distance;
            //Debug.Log("units from wall = " + unitsFromWall);
        }

        //Enables dash every "dashDuration" seconds
        if (!canDash && Time.time >= dashCooldown)
        {
            canDash = true;
        }

        //If dash is available, right-click = dash
        if (canDash && !facingWall && (currentDirection.x != 0 || currentDirection.y != 0) && Input.GetMouseButtonDown(1))
        {
            Dash();
        }

        //Enables movement after dashing
        if (!canMove && Time.time >= dashRecoveryCooldown)
        {
            enableMovement();
        }

        Debug.DrawRay(transform.position, currentDirection, Color.blue);
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        Vector3 currentPosition = transform.position;

        //Main movement logic
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //Determine Direction based on position
        currentDirection.x = (Mathf.Round(currentPosition.x * 10f) / 10f) - (Mathf.Round(lastPosition.x * 10f) / 10f);
        currentDirection.y = (Mathf.Round(currentPosition.y * 10f) / 10f) - (Mathf.Round(lastPosition.y * 10f) / 10f);
        if (currentDirection.x != 0f)
            currentDirection.x = Mathf.Sign(currentDirection.x)*1;
        if (currentDirection.y != 0f)
            currentDirection.y = Mathf.Sign(currentDirection.y)*1;
            //Debug.Log("Current Direction (in ones): " + currentDirection);

        lastPosition = currentPosition;
    }

    void Dash()
    {
        canDash = false;
        disableMovement();

        //Dash Logic
        if (unitsFromWall < dashDistance)   //Checks if close to wall (if yes, shorten dash distance accordingly)
        {
            if (currentDirection.x != 0 && currentDirection.y == 0 || currentDirection.x == 0 && currentDirection.y != 0)   //if horizontal or vertical dash
                transform.position += currentDirection * unitsFromWall;
            else
                transform.position += currentDirection * unitsFromWall / Mathf.Sqrt(2);                                     //if diagonal dash
        }
        else    //normal dash distance
        {
            if (currentDirection.x != 0 && currentDirection.y == 0 || currentDirection.x == 0 && currentDirection.y != 0)
                transform.position += currentDirection * dashDistance;
            else
                transform.position += currentDirection * dashDistance / Mathf.Sqrt(2);
        }

        //end dash logic

        dashCooldown = Time.time + dashRate;
        dashRecoveryCooldown = Time.time + dashRecovery;
    }
    
    /*
    private static void CreateDashEffect(Vector3 position, Vector3 dir, float dashSize)
    {
        Transform dashTransform = Instantiate(GameAssets.i.pfDashEffect, position, Quaternion.identity);
        dashTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        dashTransform.localScale = new Vector3(dashSize / 35f, 1, 1);
    }
    */

    public void enableMovement()
    {
        canMove = true;
    }

    public void disableMovement()
    {
        canMove = false;
    }

    public Vector3 getCurrentDirection()
    {
        return currentDirection;
    }
}
