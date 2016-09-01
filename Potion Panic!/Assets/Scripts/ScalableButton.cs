using UnityEngine;
using System.Collections;

public class ScalableButton : MonoBehaviour {

    public float targetScreenHeight = 2560;
    public float targetScreenWidth = 1620;
    public float yRatio;
    public float xRatio;
    public float aspectDifference;
    public int pixelsPerUnit = 100;
    public int screenHeight;
    public int screenWidth;
    public RectTransform tran;

    // Use this for initialization
    void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        yRatio = screenHeight / targetScreenHeight;
        xRatio = screenWidth / targetScreenWidth;
        tran = GetComponent<RectTransform>();

        tran.position = new Vector3(tran.position.x * xRatio, tran.position.y * yRatio, 0);
        tran.localScale = new Vector3(tran.localScale.x * xRatio, tran.localScale.y * yRatio, 0);


    }

    // Update is called once per frame
    void Update () {
	
	}
}
