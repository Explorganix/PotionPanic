using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PhysicsObject : MonoBehaviour {
    //Self References
    Rigidbody2D body;

    //Adjustable Parameters
    public float bounceHeight;
    public float bounceTime;
    public float fallSpeed;
    public int numRotations;

    //External Object References
    public Vector3 fallingVector;
    //public Vector3 leftTarget;
    //public Vector3 midTarget;
    //public Vector3 rightTarget;
    public Trampoline trampoline;
    public List<Vector3> targetVectors;

    //Control Variables
    protected Vector3 rotationDirection;
    protected bool movingLeft;
    protected bool rotating;
    protected bool falling;
    protected bool bouncing;
    protected int targetVectorIndex;
    protected int lastTargetVectorIndex;

    protected virtual void Awake()
    {
        falling = true;
        bouncing = false;
        bounceHeight = 14f;
        bounceTime = 4f;
        movingLeft = false;
        fallingVector = new Vector3(0, fallSpeed, 0);
        rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;
        body = GetComponent<Rigidbody2D>();
        trampoline = GameObject.FindGameObjectWithTag("Trampoline").GetComponent<Trampoline>();

    }
    // Use this for initialization
    protected virtual void Start()
    {
        //targetVectors.Add(leftTarget);
        //targetVectors.Add(midTarget);
        //targetVectors.Add(rightTarget);
        targetVectors = trampoline.GetPositions();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (falling)
        {
            body.MovePosition(transform.position + fallingVector * Time.deltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Trampoline")
        {
            UpdateRotation();
            UpdateBounce();
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

    private void UpdateRotation()
    {
        if (!rotating)
        {
            StartCoroutine("Rotate");
        }
    }

    IEnumerator Bounce()
    {
        Vector3 dest = targetVectors[targetVectorIndex];
        float time = bounceTime;

        if (bouncing) yield break;

        bouncing = true;
        Vector3 startPos = transform.position;
        float timer = 0.0f;

        while (timer <= 1f && bouncing)
        {
            float height = Mathf.Sin(Mathf.PI * timer) * bounceHeight;
            body.MovePosition(Vector3.Lerp(startPos, dest, timer) + Vector3.up * height);
            timer += Time.deltaTime / time;
            yield return null;
        }
        bouncing = false;
    }

    private void UpdateBounce()
    {
        falling = false;
        StopCoroutine("Bounce");
        bouncing = false;
        lastTargetVectorIndex = targetVectorIndex;
        if (lastTargetVectorIndex <= 0)
        {
            movingLeft = false;
        }
        else if (lastTargetVectorIndex >= 2)
        {
            movingLeft = true;
        }
        targetVectorIndex += (movingLeft) ? -1 : 1;
        rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;
        StartCoroutine("Bounce");
    }
}
