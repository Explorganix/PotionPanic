using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsSceneManager : MonoBehaviour {

    public Slider diffSlider;
    public SessionManager session;
    public int difficultyLevel;
	// Use this for initialization
    void AWake()
    {

    }
	void Start () {
        session = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        difficultyLevel = session.GetDifficultyLevel();
        diffSlider.value = difficultyLevel;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void SetDifficultyLevel()
    {
        difficultyLevel = (int)diffSlider.value;
        session.SetDifficultyLevel(difficultyLevel);
    }
}
