﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySprayPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollision2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Insert code of adding the Body Spray to your inventory here!
            Debug.Log("woop BodySpray");
            //Destroy(gameObject);
        }
    }

}
