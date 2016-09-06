using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trampoline : MonoBehaviour {

    public Vector3 leftPosition;
    public Vector3 midPosition;
    public Vector3 rightPosition;
    public List<Vector3> positions;
    public Camera cam;
    public float camHeight;
    public float camWidth;
    public int positionIndex;


    void Awake()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = 2f * camHeight * cam.aspect;
        leftPosition = new Vector3(-4.8f, -Camera.main.orthographicSize / 2, 0);
        midPosition = new Vector3(0, -Camera.main.orthographicSize / 2, 0);
        rightPosition = new Vector3(4.8f, -Camera.main.orthographicSize / 2, 0);

        positions.Add(leftPosition);
        positions.Add(midPosition);
        positions.Add(rightPosition);

        transform.position = midPosition;
        positionIndex = 1;
    }
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            positionIndex++;
            if (positionIndex > 2)
            {
                positionIndex = 0;
            }
            transform.position = positions[positionIndex];
        }
	}

    public List<Vector3> GetPositions()
    {
        return positions.GetRange(0,positions.Count);
    }
}
