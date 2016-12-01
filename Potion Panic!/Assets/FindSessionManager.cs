using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FindSessionManager : MonoBehaviour
{
  public SessionManager sessionManager;
  public string levelToLoad;
  public bool quickLoad;

  // Use this for initialization
  void Start ()
  {
    sessionManager = GameObject.FindGameObjectWithTag ("SessionManager").GetComponent<SessionManager> ();
    GetComponent<Button> ().onClick.AddListener (LoadLevel);
	
  }

  private void LoadLevel ()
  {
    if (!quickLoad) {
      sessionManager.LoadLevel (levelToLoad);
    } else {
      sessionManager.QuickLoadScene (levelToLoad);
    }

  }
}
