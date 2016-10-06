using UnityEngine;
using System.Collections;

public class ResizablePanel : MonoBehaviour {


    public float resizeSpeed;
    public float widthMin;
    public float widthMax;
    public float heightMin;
    public float heightMax;

    public RectTransform rectTrans;
    public Vector2 minSize;
    public Vector2 maxSize;

	// Use this for initialization
	void Start () {
        rectTrans = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shrink()
    {
    }

    public void Grow()
    {
        while (rectTrans.sizeDelta.x < widthMax || rectTrans.sizeDelta.y < heightMax)
        {
            Vector2 sizeDelta = rectTrans.sizeDelta;
            Vector2 resizeValue = new Vector2(resizeSpeed, resizeSpeed);
            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
            sizeDelta = new Vector2(Mathf.Clamp(sizeDelta.x, widthMin, widthMax), Mathf.Clamp(sizeDelta.y, heightMin, heightMax));
            rectTrans.sizeDelta = sizeDelta;
        }

    }
}
