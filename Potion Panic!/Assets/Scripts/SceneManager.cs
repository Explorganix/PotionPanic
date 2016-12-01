using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;

public class SceneManager : MonoBehaviour
{

  public float baseDropsPerSecond;
  public float activeDropsPerSecond;
  public float difficultyLevel;
  public float difficultyMultiplier;
  public float progressLevel;
  public float progressSpeed;
  public float dropTimer;
  public GameObject potionPrefab;
  public Vat leftVat;
  public Vat midVat;
  public Vat rightVat;
  public List<char> totalPotionRequests;
  public List<char> activePotions;
  public int totalPotionsCollected;
  public int totalAccuratePotionsCollected;
  public int totalPotionsDropped;
  public double overallAccuracy;
  public string overallAccuracyText;
  //public AccuracyGauge ag;
  public SessionManager sessionManager;
  public RectTransform pausePanelRectTransform;
  public RectTransform gameOverPanelRectTransform;
  public Canvas canvas;
  public Text scoreBoard;
  public Text pauseYourScoreText;
  public Text pauseHighScoreText;
  public Text gameOverYourScoreText;
  public Text gameOverHighScoreText;
  public int score;
  public int highScore;
  public int displayScore;
  public int basePointsPerPotion;
  public List<string> tier1Colors = new List<string> () { "red", "green", "blue" };
  public List<string> tier2Colors = new List<string> () { "yellow", "pink", "sky" };

  public AudioSource sfxPlayer;
  public AudioClip pointIncrease;
  public Image sirenFlash;
  public AudioSource sirenSound;
  public AudioClip sirenSoundClip;

  public bool failureWarning;
 
  // Use this for initialization
  void Awake ()
  {
    baseDropsPerSecond = .25f;
    progressLevel = 1f;
    progressSpeed = .02f;
    dropTimer = 0;
    totalPotionsCollected = 0;
    overallAccuracy = 100;
    overallAccuracyText = "100.0%";
    activePotions = new List<char> ();
    score = 0;
    displayScore = 0;
    totalPotionsDropped = 0;
    basePointsPerPotion = 50;
  }

  void Start ()
  {
    sessionManager = GameObject.FindGameObjectWithTag ("SessionManager").GetComponent<SessionManager> ();
    difficultyLevel = sessionManager.GetDifficultyLevel ();
    difficultyMultiplier = 1 + difficultyLevel / 20;
    activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
    UpdateTotalRequests ();
    pausePanelRectTransform = GameObject.FindGameObjectWithTag ("Pause Panel").GetComponent<RectTransform> ();
    gameOverPanelRectTransform = GameObject.FindGameObjectWithTag ("Game Over Panel").GetComponent<RectTransform> ();
    scoreBoard = GameObject.FindGameObjectWithTag ("High Score").GetComponent<Text> ();
    scoreBoard.text = "$" + displayScore;
    Time.timeScale = 1f;
    highScore = sessionManager.GetHighScore ();
    pausePanelRectTransform.SetParent (this.transform);
    gameOverPanelRectTransform.SetParent (this.transform);
  }

  // Update is called once per frame
  void Update ()
  {
    dropTimer += activeDropsPerSecond * Time.deltaTime;
    if (totalPotionRequests.Count > 0) {
      if (dropTimer >= 1) {
        dropTimer = 0;
        DropPotion ();
        totalPotionsDropped++;
        UpdateProgress ();
      }
    }
  }

  private void UpdateProgress ()
  {
    progressLevel += progressSpeed;
    activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
    switch (totalPotionsDropped) {
      case 20:
        leftVat.UpdateColorPool (totalPotionsDropped);
        midVat.UpdateColorPool (totalPotionsDropped);
        rightVat.UpdateColorPool (totalPotionsDropped);
        break;
      case 40:
        leftVat.UpdateColorPool (totalPotionsDropped);
        midVat.UpdateColorPool (totalPotionsDropped);
        rightVat.UpdateColorPool (totalPotionsDropped);
        break;
      case 60:
        leftVat.UpdateColorPool (totalPotionsDropped);
        midVat.UpdateColorPool (totalPotionsDropped);
        rightVat.UpdateColorPool (totalPotionsDropped);
        break;
    }
  }

  void DropPotion ()
  {
    GameObject newPotion = Instantiate (potionPrefab);
    int colIndex = UnityEngine.Random.Range (0, totalPotionRequests.Count);
    char colChar = totalPotionRequests [colIndex];
    activePotions.Add (colChar);
    newPotion.GetComponent<Potion> ().SetColor (colChar);
    totalPotionRequests.Remove (colChar);
  }

  internal void AddPotionRequest (char colorChar)
  {
    totalPotionRequests.Add (colorChar);
  }

  public void RemoveActivePotion (char colorChar)
  {
    activePotions.Remove (colorChar);
  }

  private List<char> GetTotalPotionRequests ()
  {
    List<char> returnList = new List<char> ();
    returnList.AddRange (leftVat.GetPotionRequests ());
    returnList.AddRange (midVat.GetPotionRequests ());
    returnList.AddRange (rightVat.GetPotionRequests ());

    return returnList;
  }

  public void ProcessVat (int vatBatchVolume, int numAccurateCollected, string requestColorString)
  {
    totalPotionsCollected += vatBatchVolume;
    totalAccuratePotionsCollected += numAccurateCollected;
    UpdateAccuracy (totalPotionsCollected, totalAccuratePotionsCollected);
    int colorTierMultiplier = 1;
    if (tier2Colors.Contains (requestColorString)) {
      colorTierMultiplier = 2;
    }
    IncreaseScore (numAccurateCollected * basePointsPerPotion * colorTierMultiplier);
  }

  private void UpdateAccuracy (int tpc, int tacp)
  {
    overallAccuracy = tacp / (double)tpc * 100;
    overallAccuracyText = overallAccuracy.ToString ("F1") + "%";
    //ag.SetAccuracy();
    if (overallAccuracy < 70) {
      GameOver ();
    } else if (overallAccuracy < 80 && !failureWarning) {
      failureWarning = true;
      StartSiren ();
      // GameOver();
    } else if (overallAccuracy >= 80 && failureWarning) {
      failureWarning = false;
      StopSiren ();
    }

  }

  private void GameOver ()
  {
    Time.timeScale = 0;
    gameOverYourScoreText.text = "$" + score;
    gameOverHighScoreText.text = "$" + highScore;
    gameOverPanelRectTransform.SetParent (canvas.transform);
    if (score > highScore) {
      highScore = score;
      sessionManager.SetHighScore (highScore);
    }
    sessionManager.musicPlayer.Stop ();
    StopSiren ();
  }

  public void UpdateTotalRequests ()
  {
    totalPotionRequests = GetTotalPotionRequests ();
    foreach (char c in activePotions) {
      totalPotionRequests.Remove (c);
    }
  }

  public double GetAccuracy ()
  {
    return overallAccuracy;
  }

  public void PauseGame ()
  {
    Time.timeScale = 0;
    pauseYourScoreText.text = "$" + score;
    pauseHighScoreText.text = "$" + highScore;
    pausePanelRectTransform.SetParent (canvas.transform);
    pausePanelRectTransform.GetComponentInChildren<ResizablePanel> ().Grow ();
    sessionManager.musicPlayer.Pause ();
    if (failureWarning) {
      StopSiren ();
    }
  }

  public void ResumeGame ()
  {
    Time.timeScale = 1;
    pausePanelRectTransform.SetParent (this.transform);
    sessionManager.musicPlayer.UnPause ();
    if (failureWarning) {
      StartSiren ();
    }
  }

  public void IncreaseScore (int points)
  {
    score += points;
    if (score > highScore) {
      highScore = score;
      sessionManager.SetHighScore (highScore);
    }
    StartCoroutine ("IncrementScoreBoard");
  }

  IEnumerator IncrementScoreBoard ()
  {
    while (displayScore < score) {
      displayScore += 5;

      if (displayScore % 40 == 0) {
        scoreBoard.fontSize -= 19;
      }
      if (displayScore % 20 == 0) {
        scoreBoard.fontSize = Mathf.Clamp (scoreBoard.fontSize + 10, 50, 109);
        sfxPlayer.PlayOneShot (pointIncrease);

      }
      scoreBoard.text = "$" + displayScore;
      yield return null;
    }

  }

  void StartSiren ()
  {
    StartCoroutine ("SirenFlashOn");
    sirenSound.Play ();
    sessionManager.musicPlayer.volume = .20f;
  }

  void StopSiren ()
  {
    StopCoroutine ("SirenFlashOn");
    StartCoroutine ("SirenFlashOff");
    sirenSound.Stop ();
    sessionManager.musicPlayer.volume = 1f;

  }


  public IEnumerator SirenFlashOn ()
  {
    while (failureWarning) {
      for (float f = 0f; f <= .2; f += 0.01f) {
        Color c = sirenFlash.color;
        c.a = f;
        sirenFlash.color = c;
        yield return new WaitForEndOfFrame ();
      }

      for (float f = .2f; f >= 0; f -= 0.01f) {
        Color c = sirenFlash.color;
        c.a = f;
        sirenFlash.color = c;
        yield return new WaitForEndOfFrame ();
      }
    }

  }

  public IEnumerator SirenFlashOff ()
  {
    for (float f = .2f; f >= 0; f -= 0.01f) {
      Color c = sirenFlash.color;
      c.a = f;
      sirenFlash.color = c;
      yield return new WaitForEndOfFrame ();
    }
  }
    
}