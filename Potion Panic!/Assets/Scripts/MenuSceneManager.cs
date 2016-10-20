using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {

    public SessionManager sm;
    public Text highScoreText;
    public int highScore;
	// Use this for initialization
	void Start () {
        sm = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        highScore = sm.GetHighScore();
        highScoreText.text = "$" + highScore;
    }

    // Update is called once per frame
    void Update () {
	    
	}



    void DisplayHighscore()
    {

    }

}
