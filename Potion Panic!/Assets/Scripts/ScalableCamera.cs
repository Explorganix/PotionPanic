using UnityEngine;
using System.Collections;

public class ScalableCamera : MonoBehaviour {

    public float targetScreenHeight = 2560;
    public float targetScreenWidth = 1620;
    public float targetAspectRatio;
    public float deviceAspectRatio;
    public float aspectDifference;
    public int pixelsPerUnit = 100;
    public int screenHeight;
    public int screenWidth;
    

    // Use this for initialization
    void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        targetAspectRatio = targetScreenWidth / targetScreenHeight;
        deviceAspectRatio = (float)Screen.width / Screen.height;
        aspectDifference = targetAspectRatio / deviceAspectRatio;
        if (deviceAspectRatio <= targetAspectRatio)
        {
            GetComponent<Camera>().orthographicSize = targetScreenHeight / 2 / pixelsPerUnit;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = targetScreenHeight / 2 / pixelsPerUnit * aspectDifference ;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
