using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// this script was written to emulate gravity between objects in a top down space shooter.

// Adding this Gravitizer script creates a gravitational pull between two colliders 
// within a given overlap circle.

// notes:
// -gravity should be off in Unity
// -any object that has mass should also have this script (asteroids, planets, ships)
// -works well with having mass auto calculate from Unity - it scales with object size

// Author: https://github.com/X3r0byte/

public class Gravitizer : MonoBehaviour {
    public float pullRadius;
    public float gravitizerMultiplier = 50;
    public float mass;
    public Vector2 size;

    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        size = transform.GetComponent<Collider2D>().bounds.size;
        mass = rb.mass;
        // NetworkServer.Spawn(gameObject);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position for debug
     //   Gizmos.color = Color.yellow;
     //   Gizmos.DrawWireSphere(transform.position, pullRadius * size.x);
    }

    public void FixedUpdate()
    {
        // scans for any objects with a 2D collider in a circle dependant on the object's size
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, pullRadius * size.x))
        {
            // calculate direction from this to target
            Vector2 forceDirection = transform.position - collider.transform.position;

            // vars for gravity calc
            float colliderMass = collider.GetComponent<Rigidbody2D>().mass;
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            float gravitationalForce = 1;

            if (distance > 0) // fixes a condition where FixedUpdate sometimes calcs distance as zero
            {
                // simple way to calc gravity GravitizerMultiplier scales the force as the gravitational constant
                gravitationalForce = (mass * colliderMass) / (float)(Math.Pow(distance, 2)) * gravitizerMultiplier;
            }

            // draws a red line between both colliders for debug, indicating pull
            Color color = Color.red;
            Debug.DrawLine(transform.position, collider.transform.position, color);

            // apply force on target towards this
            if(collider.gameObject.tag == "Player")
            {
                // Text txtgrav = GameObject.Find("UI/Canvas/GravityText").GetComponent<Text>();
                //  if (isLocalPlayer)
                //{
                  collider.transform.Find("Canvas/GravityText").GetComponent<Text>().text = gravitationalForce.ToString("N2") + " N";
                // }
                // collider.
               //  txtgrav.text = gravitationalForce.ToString();
            }

            // apply the force to the collider
            collider.GetComponent<Rigidbody2D>().AddForce((forceDirection.normalized * gravitationalForce * (Time.fixedDeltaTime)));
        }
    }
}
