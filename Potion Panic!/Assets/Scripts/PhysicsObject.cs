using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour {

    Random rand;
    //Self References
    Rigidbody2D body;

    //Adjustable Parameters
    public float bounceHeight;
    public float bounceTime;
    public float fallSpeed;
    public float numRotations;
    public List<Vector3> spawnPositions;
    public float collisionTimer;

    //External Object References
    public Vector3 fallingVector;
    public Trampoline trampoline;
    public List<Vector3> targetVectors;

    //Control Variables
    protected Vector3 rotationDirection;
    public bool movingLeft;
    protected bool rotating;
    protected bool falling;
    protected bool bouncing;
    public int targetVectorIndex;
    public int lastTargetVectorIndex;
    public int spawnIndex;

    protected virtual void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        rand = new Random();
        falling = true;
        bouncing = false;
        bounceHeight = 15f;
        bounceTime = 4f;
        fallSpeed = -10f; //between 10 and 12
        numRotations = 2.25f;
        fallingVector = new Vector3(0, fallSpeed, 0);
        body = GetComponent<Rigidbody2D>();
        trampoline = GameObject.FindGameObjectWithTag("Trampoline").GetComponent<Trampoline>();

    }
    // Use this for initialization
    protected virtual void Start()
    {
        
        targetVectors = GetTargetPositions();
        for (int i = 0; i < targetVectors.Count; i++)
        {
            targetVectors[i] += new Vector3(0, - 6, 0);
        }
        //takes the trampoline locations and adds to the Y values so they spawn above the top of the screen.
        spawnPositions = targetVectors.GetRange(0, targetVectors.Count);
        for(int i = 0; i < spawnPositions.Count; i++)
        {
            spawnPositions[i] += new Vector3(0, 30, 0);
        }
        spawnIndex = Random.Range(0, 3);
        transform.position = spawnPositions[spawnIndex];

        switch (spawnIndex)
        {
            case 0:
                movingLeft = false;
                targetVectorIndex = 0;
                lastTargetVectorIndex = 1;
                break;
            case 1:
                movingLeft = (Random.Range(0, 2) == 1) ? false : true;
                targetVectorIndex = 1;
                lastTargetVectorIndex = (movingLeft) ? 2 : 0;
                break;
            case 2:
                movingLeft = true;
                targetVectorIndex = 2;
                lastTargetVectorIndex = 1;
                break;
        }
        rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;

    }

    private List<Vector3> GetTargetPositions()
    {
        List<Vector3> returnList;
        returnList = trampoline.GetPositions();
        for(int i = 0; i < returnList.Count; i++)
        {
            returnList[i] += new Vector3(0, (GetComponent<CircleCollider2D>().radius * transform.localScale.y / 2) + trampoline.GetComponent<BoxCollider2D>().size.y, 0);
        }
        return returnList;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (falling)
        {
            body.MovePosition(transform.position + fallingVector * Time.deltaTime);
        }
        collisionTimer += .016f;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Trampoline" && collisionTimer > .5f)
        {
            collisionTimer = 0;
            UpdateBounce();
            UpdateRotation();
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
        rotationDirection = (movingLeft) ? Vector3.forward : Vector3.back;
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
        StartCoroutine("Bounce");
    }
}
