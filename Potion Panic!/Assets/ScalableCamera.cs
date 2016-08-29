using UnityEngine;
using System.Collections;

public class ScalableCamera : MonoBehaviour {
    public int pixelsPerUnit = 100;


	// Use this for initialization
	void Start () {
        GetComponent<Camera>().orthographicSize = (float)Screen.height / 2 / pixelsPerUnit;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
