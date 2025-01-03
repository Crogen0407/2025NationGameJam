using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SquareShooter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] boxes;
    
    private void Start()
    {
        var rd = new Random();
        
        foreach (var o in boxes) 
        {
            o.AddForce(new Vector2(rd.Next(100),rd.Next(100)).normalized * 100, ForceMode2D.Force);
        }
    }
}
