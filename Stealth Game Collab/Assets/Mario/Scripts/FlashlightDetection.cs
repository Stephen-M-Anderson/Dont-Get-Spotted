﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightDetection : MonoBehaviour
{
    public EnemyScript enemyScript;

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.tag == "Player")
        {
            
        }
    }

    void OnTriggerExit2D(Collider2D o)
    {
        if (o.gameObject.tag == "Player")
        {
            
        }
    }
}