using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

    public Vector3 movementVector;
    public Vector3 targetVector;

    public bool fallingStraight = true;
    public bool bouncing = false;
    public float bounceHeight = 8f;

    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        if (fallingStraight)
        {
            body.MovePosition(transform.position + movementVector * Time.deltaTime);
            //transform.Translate(movementVector * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Trampoline")
        {
                fallingStraight = false;
                bouncing = false;
                StopCoroutine("Bounce");        
                StartCoroutine("Bounce");
            
        }
    }

    IEnumerator Bounce()
    {
            Vector3 dest = targetVector;
            float time = 2.5f;
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
}
