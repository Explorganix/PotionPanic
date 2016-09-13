using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {

    SceneManager sm;
	// Use this for initialization
	void Start () {
        sm = new SceneManager();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void LoadLevel(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
