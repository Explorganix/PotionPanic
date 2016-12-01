using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{

  public static SessionManager instance = null;

  public int difficultyLevel;
  public Image fadeMask;
  public int highScore;

  public AudioSource musicPlayer;
  public AudioClip playSceneMusic;
  public AudioClip menuSceneMusic;
  public AudioClip gameOverMusic;

  public Camera mainCamera;
  public Canvas canvas;

  void Awake ()
  {
    //make singleton
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy (gameObject);
    }
    DontDestroyOnLoad (gameObject);

    GetPlayerPrefs ();
    musicPlayer = GetComponent<AudioSource> ();
    canvas = GetComponentInChildren<Canvas> ();
  }

  public int GetDifficultyLevel ()
  {
    return difficultyLevel;
  }

  // Use this for initialization
  void Start ()
  {
    LoadLevel ("Main_Menu");
  }

  public void SetDifficultyLevel (int lvl)
  {
    difficultyLevel = lvl;
    PlayerPrefs.SetInt ("DifficultyLevel", difficultyLevel);
  }

  public void LoadLevel (string sceneName)
  {

    StartCoroutine (TransitionScenes (sceneName));


  }

  public int GetHighScore ()
  {
    return highScore;
  }

  public void SetHighScore (int score)
  {

    highScore = score;
    Debug.Log ("Setting HighScore in Session Manager: " + highScore);
    PlayerPrefs.SetInt ("HighScore", highScore);
  }


  IEnumerator FadeMaskOut ()
  {
    Debug.Log ("Fading out started");
    if (fadeMask.color.a > 0.01f) {
      yield return new WaitForSeconds (1f);
      for (float f = 1f; f > 0; f -= 0.01f) {
        Color c = fadeMask.color;
        c.a = f;
        fadeMask.color = c;
        yield return new WaitForEndOfFrame ();
      }
      yield return new WaitForSeconds (.25f);
    }
    Debug.Log ("Fading  out complete");
  }

  IEnumerator FadeMaskIn ()
  {    
    Debug.Log ("Fading  in started");    
    for (float f = 0.01f; f < 1f; f += 0.01f) {
      Color c = fadeMask.color;
      c.a = f;
      fadeMask.color = c;
      yield return new WaitForEndOfFrame ();
    }
    Debug.Log ("Fading  in complete");
      
    yield return new WaitForSeconds (.25f);

  }

  void GetPlayerPrefs ()
  {
    highScore = PlayerPrefs.GetInt ("HighScore");
    difficultyLevel = PlayerPrefs.GetInt ("DifficultyLevel");
  }

  IEnumerator TransitionScenes (string sceneName)
  {
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    StartCoroutine ("FadeMaskOut");
    yield return new WaitForSeconds (3f); 
    StartCoroutine ("FadeMaskIn");
    yield return new WaitForSeconds (3f);
    QuickLoadScene (sceneName);

    StartCoroutine ("FadeMaskOut");
    yield return new WaitForSeconds (3f);
    canvas.renderMode = RenderMode.WorldSpace;

  }

  public void QuickLoadScene (string sceneName)
  {
    string previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
    UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);

    switch (sceneName) {

      case "Play_Scene":
        //if (previousScene != "Play_Scene") {
        musicPlayer.clip = playSceneMusic;
        musicPlayer.Play ();
        //}
        break;
      case "Main_Menu":
        if (previousScene == "Play_Scene" || previousScene == "Splash_Screen") {
          musicPlayer.clip = menuSceneMusic;
          musicPlayer.Play ();
        }
        break;
    }
  
  }

}
