﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{

  public static SessionManager instance = null;

  public int difficultyLevel;
  public Image splashArt;
  public SpriteRenderer menuArt;
  public int highScore;

  public AudioSource musicPlayer;
  public AudioClip playSceneMusic;
  public AudioClip menuSceneMusic;
  public AudioClip gameOverMusic;

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
  }

  public int GetDifficultyLevel ()
  {
    return difficultyLevel;
  }

  // Use this for initialization
  void Start ()
  {
    //splashArt = GameObject.FindGameObjectWithTag("Splash Art").GetComponent<SpriteRenderer>();
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Splash_Screen") {
      StartCoroutine ("SplashScreenDelay");
    }
  }

  public void SetDifficultyLevel (int lvl)
  {
    difficultyLevel = lvl;
    PlayerPrefs.SetInt ("DifficultyLevel", difficultyLevel);
  }

  public void LoadLevel (string sceneName)
  {

    string previousScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName);

    switch (sceneName) {
      
      case "Play_Scene":
        if (previousScene != "Play_Scene") {
          musicPlayer.clip = playSceneMusic;
          musicPlayer.Play ();
        }
        break;
      case "Main_Menu":
        if (previousScene == "Play_Scene") {
          musicPlayer.clip = menuSceneMusic;
          musicPlayer.Play ();
        }
        break;
    }
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

  IEnumerator SplashScreenDelay ()
  {
    yield return new WaitForSeconds (5f);
    StartCoroutine ("FadeOut");

  }

  IEnumerator FadeMaskIn ()
  {
    for (float f = 1f; f >= 0; f -= 0.02f) {
      Color c = splashArt.color;
      c.a = f;
      splashArt.color = c;
      yield return new WaitForEndOfFrame ();
    }
    yield return new WaitForSeconds (.25f);
    UnityEngine.SceneManagement.SceneManager.LoadScene ("Main_Menu");
    //menuArt = GameObject.FindGameObjectWithTag("Menu Art").GetComponent<SpriteRenderer>();
  }

  IEnumerator FadeMaskOut ()
  {
    yield return null;
  }

  void GetPlayerPrefs ()
  {
    highScore = PlayerPrefs.GetInt ("HighScore");
    difficultyLevel = PlayerPrefs.GetInt ("DifficultyLevel");
  }

}
