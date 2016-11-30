using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FindSessionManager : MonoBehaviour {
  public SessionManager sessionManager;
  public string levelToLoad;

	// Use this for initialization
	void Start () {
    sessionManager = GameObject.FindGameObjectWithTag ("SessionManager").GetComponent<SessionManager>();
    GetComponent<Button> ().onClick.AddListener(LoadLevel);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  private void LoadLevel(){
    sessionManager.LoadLevel (levelToLoad);
  }
}
