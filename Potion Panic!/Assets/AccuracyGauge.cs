using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccuracyGauge : MonoBehaviour {

    public double currentAccuracy;
    public SceneManager sm; 
	// Use this for initialization
	void Start () {
        SetAccuracy();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetAccuracy()
    {
        currentAccuracy = sm.GetAccuracy();
        GetComponent<Image>().fillAmount = (float)currentAccuracy / 100;
    }
}
