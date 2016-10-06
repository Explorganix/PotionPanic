using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trampoline : MonoBehaviour
{

    public Vector3 leftPosition;
    public Vector3 midPosition;
    public Vector3 rightPosition;
    public List<Vector3> positions;
    public Camera cam;
    public float camHeight;
    public float camWidth;
    public int positionIndex;
    public float moveFrequency;
    public float moveTimer;

    void Awake()
    {
        moveFrequency = .1f;
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = 2f * camHeight * cam.aspect;
        leftPosition = new Vector3(-4.8f, -Camera.main.orthographicSize / 2 + .7f, 0);
        midPosition = new Vector3(0, -Camera.main.orthographicSize / 2 + .7f, 0);
        rightPosition = new Vector3(4.8f, -Camera.main.orthographicSize / 2 + .7f, 0);

        positions.Add(leftPosition);
        positions.Add(midPosition);
        positions.Add(rightPosition);

        transform.position = midPosition;
        positionIndex = 1;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > moveFrequency)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    positionIndex--;
                    if (positionIndex < 0)
                    {
                        positionIndex = 0;
                    }
                    transform.position = positions[positionIndex];
                }
                else
                {
                    positionIndex++;
                    if (positionIndex > 2)
                    {
                        positionIndex = 2;
                    }
                    transform.position = positions[positionIndex];
                }
                moveTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                positionIndex--;
                if (positionIndex < 0)
                {
                    positionIndex = 0;
                }
                transform.position = positions[positionIndex];
                moveTimer = 0;

            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                positionIndex++;
                if (positionIndex > 2)
                {
                    positionIndex = 2;
                }
                transform.position = positions[positionIndex];
                moveTimer = 0;

            }

#else
        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
             foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2)
            {
                positionIndex--;
                if (positionIndex < 0)
                {
                    positionIndex = 0;
                }
                transform.position = positions[positionIndex];
            }

            else if(touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
            {
                positionIndex++;
                if (positionIndex > 2)
                {
                    positionIndex = 2;
                }
                transform.position = positions[positionIndex];
            }
        }
            moveTimer = 0;
        }

       
#endif

        }
    }

    public List<Vector3> GetPositions()
    {
        return positions.GetRange(0, positions.Count);
    }
}
