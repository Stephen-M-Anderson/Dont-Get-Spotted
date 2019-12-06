﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalking : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode interact = KeyCode.E;
    public KeyCode dash = KeyCode.Space;
    public KeyCode cover = KeyCode.LeftControl;

    [Header("Movement settings")]
    public float moveSpeed;
    public float maxSpeed;

    public Rigidbody2D rb;
    public CoverScript cc;
    public bool peeking;

    // Start is called before the first frame 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CoverScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Movement
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(0.0f - Camera.main.transform.position.z);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        direction = direction.normalized;
        if(!cc.inCover)
            transform.up = direction;
        mousePosition.z = -10;

        //Vector2 newMovement;
        if (!cc.inCover)
        {
            
            if (Input.GetKey(moveLeft) && (!Input.GetKey(moveUp) || !Input.GetKey(moveDown)))
            {
                if (rb.velocity.x > 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.x = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.magnitude < maxSpeed)
                    rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);
            }
            else if (Input.GetKey(moveLeft) && Input.GetKey(moveUp))
            {
                if (rb.velocity.x > 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.x = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.y < 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.y = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.x == 0 - maxSpeed) // if the player is moving left at max speed, stop them momentarily 
                {
                    Vector2 stop = rb.velocity;
                    stop = Vector2.zero;
                    rb.velocity = stop;
                }
                if (rb.velocity.magnitude < maxSpeed)
                {
                    Vector2 d = Vector2.left + Vector2.up;
                    d = d.normalized;
                    rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);
                }
            }
            if (Input.GetKey(moveRight))
            {
                if (rb.velocity.x < 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.x = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.magnitude < maxSpeed)
                    rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
                if (Input.GetKey(moveUp))
                {
                    if (rb.velocity.y < 0)
                    {
                        Vector2 stop = rb.velocity;
                        stop.y = 0;
                        rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                    }
                    if (rb.velocity.magnitude < maxSpeed)
                        rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
                }
                if (Input.GetKey(moveDown))
                {
                    if (rb.velocity.y > 0)
                    {
                        Vector2 stop = rb.velocity;
                        stop.y = 0;
                        rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                    }
                    if (rb.velocity.magnitude < maxSpeed)
                        rb.AddForce(Vector2.down * moveSpeed, ForceMode2D.Impulse);
                }
            }
            if (Input.GetKey(moveUp))
            {
                if (rb.velocity.y < 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.y = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.magnitude < maxSpeed)
                    rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
            }
            if (Input.GetKey(moveDown))
            {
                if (rb.velocity.y > 0)
                {
                    Vector2 stop = rb.velocity;
                    stop.y = 0;
                    rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                }
                if (rb.velocity.magnitude < maxSpeed)
                    rb.AddForce(Vector2.down * moveSpeed, ForceMode2D.Impulse);
            }
            if  (!Input.GetKey(moveLeft) && rb.velocity.x < 0f) // if the player is moving left but is not pushing left, stop them
            {
                Vector2 stop = rb.velocity;
                stop.x = 0f;
                rb.velocity = stop;
            }
            if (!Input.GetKey(moveRight) && rb.velocity.x > 0f) // if the player is moving right but is not pushing right, stop them
            {
                Vector2 stop = rb.velocity;
                stop.x = 0f;
                rb.velocity = stop;
            }
            if (!Input.GetKey(moveUp) && rb.velocity.y > 0f) // if the player is moving up but is not pushing up, stop them
            {
                Vector2 stop = rb.velocity;
                stop.y = 0f;
                rb.velocity = stop;
            }
            if (!Input.GetKey(moveDown) && rb.velocity.y < 0f) // if the player is moving down but is not pushing down, stop them
            {
                Vector2 stop = rb.velocity;
                stop.y = 0f;
                rb.velocity = stop;
            }
        }
        else if (cc.inCover) // if the player is in cover
        {
            if (cc.coverSide == 1 || cc.coverSide == 3) // player is against horizontal cover (they are facing up or down)
            {
                if (cc.atCoverCorner) // if the player is at a corner of their cover
                {
                    if (cc.whichCornerBool == true) // if the player is at the top or bottom right corner, player can only move left
                    {
                        if (rb.velocity.x > 0)
                        {
                            rb.velocity = Vector2.zero;
                        }
                        if (Input.GetKey(moveLeft))
                        {
                            //newMovement = new Vector2(rb.position.x - (moveSpeed * Time.deltaTime), rb.position.y);
                            //rb.position = newmovement;
                            if (rb.velocity.magnitude < maxSpeed)
                                rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                    else                            // if the player is at the top or bottom left corner, player can only move right
                    {
                        if (rb.velocity.x < 0)
                        {
                            rb.velocity = Vector2.zero;
                        }
                        if (Input.GetKey(moveRight))
                        {
                            //newMovement = new Vector2(rb.position.x + (moveSpeed * Time.deltaTime), rb.position.y);
                            //rb.position = newmovement;
                            if (rb.velocity.magnitude < maxSpeed)
                                rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    } 
                }
                else                                // the player is not at a corner of their cover, they can move left or right
                {
                    if (Input.GetKey(moveRight))
                    {
                        //newMovement = new Vector2(rb.position.x + (moveSpeed * Time.deltaTime), rb.position.y);
                        //rb.position = newmovement;
                        if (rb.velocity.x < 0)
                        {
                            Vector2 stop = rb.velocity;
                            stop.x = 0;
                            rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                        }
                        if (rb.velocity.magnitude < maxSpeed)
                            rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
                    }
                    if (Input.GetKey(moveLeft))
                    {
                        //newMovement = new Vector2(rb.position.x - (moveSpeed * Time.deltaTime), rb.position.y);
                        //rb.position = newmovement;
                        if (rb.velocity.x > 0)
                        {
                            Vector2 stop = rb.velocity;
                            stop.x = 0;
                            rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                        }
                        if (rb.velocity.magnitude < maxSpeed)
                            rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);
                    }
                }
            }
            else if (cc.coverSide == 2 || cc.coverSide == 4) // player is against verticle cover
            {
                if (cc.atCoverCorner) // if the player is at a corner of their cover
                {
                    if (cc.whichCornerBool == true) // if the player is at the top left or right corner, player can only move down
                    {
                        if (rb.velocity.y > 0)
                        {
                            //Vector2 stop = rb.velocity;
                            //stop.y = 0;
                            //rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                            rb.velocity = Vector3.zero;
                        }
                        if (Input.GetKey(moveDown))
                        {
                            //newMovement = new Vector2(rb.position.x, rb.position.y - (moveSpeed * Time.deltaTime));
                            //rb.position = newmovement;
                            if (rb.velocity.magnitude < maxSpeed)
                                rb.AddForce(Vector2.down * moveSpeed, ForceMode2D.Impulse);
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                    else                            // if the player is at the bottom left or right corner, player can only move up
                    {
                        if (rb.velocity.y < 0)
                        {
                            rb.velocity = Vector3.zero;
                        }
                        if (Input.GetKey(moveUp))
                        {
                            //newMovement = new Vector2(rb.position.x, rb.position.y + (moveSpeed * Time.deltaTime));
                            //rb.position = newmovement;
                            if (rb.velocity.magnitude < maxSpeed)
                                rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                }
                else                                // the player is not at a corner of their cover, they can move up or down
                {
                    if (Input.GetKey(moveUp))
                    {
                        //newMovement = new Vector2(rb.position.x, rb.position.y + (moveSpeed * Time.deltaTime));
                        //rb.position = newmovement;
                        if (rb.velocity.y < 0)
                        {
                            Vector2 stop = rb.velocity;
                            stop.y = 0;
                            rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                        }
                        if (rb.velocity.magnitude < maxSpeed)
                            rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
                    }
                    if (Input.GetKey(moveDown))
                    {
                        //newMovement = new Vector2(rb.position.x, rb.position.y - (moveSpeed * Time.deltaTime));
                        //rb.position = newmovement;
                        if (rb.velocity.y > 0)
                        {
                            Vector2 stop = rb.velocity;
                            stop.y = 0;
                            rb.velocity = Vector2.Lerp(rb.velocity, stop, 0.5f);
                        }
                        if (rb.velocity.magnitude < maxSpeed)
                            rb.AddForce(Vector2.down * moveSpeed, ForceMode2D.Impulse);
                    }
                }
            }
        }
        if (Input.GetKey(moveRight) == false && Input.GetKey(moveLeft) == false && Input.GetKey(moveUp) == false && Input.GetKey(moveDown) == false && rb.velocity != Vector2.zero) // if no input is being pressed and the player is moving, stop them
        {
            Vector2 stop = Vector2.zero;
            rb.velocity = stop;
        }
        /*         else if (cc.inCover) // if the player is in cover
        {
            if (cc.coverSide == 1 || cc.coverSide == 3) // player is against horizontal cover (they are facing up or down)
            {
                if (cc.atCoverCorner) // if the player is at a corner of their cover
                {
                    if (cc.whichCornerBool == true) // if the player is at the top or bottom right corner, player can only move left
                    {
                        if (Input.GetKey(moveLeft))
                        {
                            //newMovement = new Vector2(rb.position.x - (moveSpeed * Time.deltaTime), rb.position.y);
                            //rb.position = newmovement;
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                    else                            // if the player is at the top or bottom left corner, player can only move right
                    {
                        if (Input.GetKey(moveRight))
                        {
                            //newMovement = new Vector2(rb.position.x + (moveSpeed * Time.deltaTime), rb.position.y);
                            //rb.position = newmovement;
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    } 
                }
                else                                // the player is not at a corner of their cover, they can move left or right
                {
                    if (Input.GetKey(moveRight))
                    {
                        //newMovement = new Vector2(rb.position.x + (moveSpeed * Time.deltaTime), rb.position.y);
                        //rb.position = newmovement;
                    }
                    if (Input.GetKey(moveLeft))
                    {
                        //newMovement = new Vector2(rb.position.x - (moveSpeed * Time.deltaTime), rb.position.y);
                        //rb.position = newmovement;
                    }
                }
            }
            else if (cc.coverSide == 2 || cc.coverSide == 4) // player is against verticle cover
            {
                if (cc.atCoverCorner) // if the player is at a corner of their cover
                {
                    if (cc.whichCornerBool == true) // if the player is at the top left or right corner, player can only move down
                    {
                        if (Input.GetKey(moveDown))
                        {
                            //newMovement = new Vector2(rb.position.x, rb.position.y - (moveSpeed * Time.deltaTime));
                            //rb.position = newmovement;
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                    else                            // if the player is at the bottom left or right corner, player can only move up
                    {
                        if (Input.GetKey(moveUp))
                        {
                            //newMovement = new Vector2(rb.position.x, rb.position.y + (moveSpeed * Time.deltaTime));
                            //rb.position = newmovement;
                        }
                        if (Input.GetKey(interact))
                        {
                            peeking = true;
                        }
                        else
                            peeking = false;
                    }
                }
                else                                // the player is not at a corner of their cover, they can move up or down
                {
                    if (Input.GetKey(moveUp))
                    {
                        //newMovement = new Vector2(rb.position.x, rb.position.y + (moveSpeed * Time.deltaTime));
                        //rb.position = newmovement;
                    }
                    if (Input.GetKey(moveDown))
                    {
                        //newMovement = new Vector2(rb.position.x, rb.position.y - (moveSpeed * Time.deltaTime));
                        //rb.position = newmovement;
                    }
                }
            }
        }
        */
    }
}
