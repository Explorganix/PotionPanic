using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Potion : MonoBehaviour {

    public Vector3 movementVector;
    public Vector3 leftVatPos;
    public Vector3 midVatPos;
    public Vector3 rightVatPos;
    public List<Vector3> vatVectors;
    public int targetVectorIndex;
    public int lastVectorIndex;
    public int numRotations;
    public bool movingLeft;
    public bool rotating;
    public Vector3 rotationDirection;

    public bool fallingStraight = true;
    public bool bouncing = false;
    public float bounceHeight = 8f;
    public float bounceTime;

    Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        vatVectors.Add(leftVatPos);
        vatVectors.Add(midVatPos);
        vatVectors.Add(rightVatPos);
        body = GetComponent<Rigidbody2D>();
        movingLeft = false;
        rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;
    }

    // Update is called once per frame
    void Update()
    {
        if (fallingStraight)
        {
            body.MovePosition(transform.position + movementVector * Time.deltaTime);
        }

    }


    IEnumerator Rotate()
    {
        if (rotating) yield break;
        rotating = true;
        while (rotating)
        {
            transform.Rotate(rotationDirection, 360 * numRotations * Time.deltaTime / bounceTime);
            yield return null;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Trampoline")
        {
            if (!rotating)
            {
                StartCoroutine("Rotate");
            }
            fallingStraight = false;
            bouncing = false;
            StopCoroutine("Bounce");
            lastVectorIndex = targetVectorIndex;
            if(lastVectorIndex <= 0)
            {
                movingLeft = false;
            }
            else if(lastVectorIndex >= 2)
            {
                movingLeft = true;
            }

            targetVectorIndex += (movingLeft) ?  -1 : 1;
            rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;
            //if (movingLeft)
            //{              
            //    targetVectorIndex--;
            //}
            //else
            //{
            //    targetVectorIndex++;
            //}
            StartCoroutine("Bounce");

        }
    }


    IEnumerator Bounce()
    {
        Vector3 dest = vatVectors[targetVectorIndex];
        float time = bounceTime;
        if (bouncing) yield break;
        
        bouncing = true;
        Vector3 startPos = transform.position;
        float timer = 0.0f;

        while (timer <= 1f && bouncing)
        {
            float height = Mathf.Sin(Mathf.PI * timer) * bounceHeight;
            body.MovePosition(Vector3.Lerp(startPos, dest, timer) + Vector3.up * height);
            //transform.Rotate(Vector3.back, 720 * (Time.deltaTime / bounceTime));

            timer += Time.deltaTime / time;
            yield return null;
        }
        bouncing = false;
    }
}
