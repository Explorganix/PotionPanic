using System;
using UnityEngine;
using System.Collections.Generic;


public class Vat : MonoBehaviour
{

  public int targetVolume;
  public int volumeMax;
  public int volumeMin;
  public List<char> potionRequests;
  public List<char> potionsCollected;
  public SceneManager sceneManager;
  public int numAccuratePotionsCollected;
  public double vatAccuracy;
  public string vatAccuracyText;
  public Color targetColor;
  public Color collectedColor;
  public SpriteRenderer targetColorSprite;
  public SpriteRenderer collectedColorSprite;
  public Transform collectedColorTran;
  public float fullCollectedColorSize;
  public float currentCollectedColorSize;
  public List<string> colorPool = new List<string> () { "red", "green", "blue" };
  public List<string> tier2Colors = new List<string> () { "yellow", "pink", "sky" };
  string requestColorString = "blue";
  public AudioClip potionCollectCorrect;
  public AudioClip potionColletIncorrect;
  public List<AudioClip> potionSounds;
  public AudioSource sfxPlayer;

  public ParticleSystem splashSystem;

  void Awake ()
  {
    fullCollectedColorSize = 10.5f;
    volumeMax = 4;
    volumeMin = 4;
    numAccuratePotionsCollected = 0;
    potionsCollected = new List<char> ();
    potionSounds = new List<AudioClip> (){ potionCollectCorrect, potionColletIncorrect };
    sfxPlayer = GetComponent<AudioSource> ();
    Initialize ();
  }

  void Start ()
  {
    sceneManager = GameObject.FindGameObjectWithTag ("SceneManager").GetComponent<SceneManager> ();
  }

  //scales up complexity as progress is made in the game
  List<char> CreatePotionRequests ()
  {
    List<char> returnList = new List<char> ();
    int colorIndex = UnityEngine.Random.Range (0, colorPool.Count);
    requestColorString = colorPool [colorIndex];
    switch (requestColorString) {
    //tier 1 for early game
      case "red":
        for (int i = 0; i < targetVolume; i++) {
          returnList.Add ('r');
        }
        break;

      case "green":
        for (int i = 0; i < targetVolume; i++) {
          returnList.Add ('g');
        }
        break;
      case "blue":
        for (int i = 0; i < targetVolume; i++) {
          returnList.Add ('b');
        }
        break;

    //tier 2 for mid game
      case "yellow":
        for (int i = 0; i < targetVolume; i++) {
          if (i % 2 == 0) {
            returnList.Add ('r');
          } else {
            returnList.Add ('g');
          }
        }
        break;
      case "pink":
        for (int i = 0; i < targetVolume; i++) {
          if (i % 2 == 0) {
            returnList.Add ('r');
          } else {
            returnList.Add ('b');
          }
        }
        break;
      case "sky":
        for (int i = 0; i < targetVolume; i++) {
          if (i % 2 == 0) {
            returnList.Add ('b');
          } else {
            returnList.Add ('g');
          }
        }
        break;

    //make potion blue if nothing else
      default:
        for (int i = 0; i < targetVolume; i++) {
          returnList.Add ('b');
        }
        break;
    }
    return returnList;
  }


  public List<char> GetPotionRequests ()
  {
    return potionRequests.GetRange (0, potionRequests.Count);
  }

  public void ProcessPotion (char colorChar)
  {

    sceneManager.RemoveActivePotion (colorChar);
    potionsCollected.Add (colorChar);
    UpdateColorCollected ();
    UpdateVolumeCollected ();

    if (potionRequests.Contains (colorChar)) {
      potionRequests.Remove (colorChar);
      numAccuratePotionsCollected++;
      sfxPlayer.PlayOneShot (potionSounds [0]);
      splashSystem.Emit (20);

    } else {
      sceneManager.AddPotionRequest (colorChar);
      sfxPlayer.PlayOneShot (potionSounds [1]);
      splashSystem.Emit (20);

    }
    UpdateAccuracy ();
    CheckIfFull ();
    Debug.Log ("Potion being processed color is :" + colorChar);
  }

  private void UpdateVolumeCollected ()
  {
    currentCollectedColorSize = fullCollectedColorSize / targetVolume * potionsCollected.Count;
    collectedColorTran.localScale = new Vector3 (4.6f, currentCollectedColorSize, 0);
  }

  private void UpdateColorCollected ()
  {
    int numReds = 0;
    int numGreens = 0;
    int numBlues = 0;
    float colorNormalizer = 2.0f / potionsCollected.Count; //ensures that the collected color has accurate proportions of the potions collected thus far
    foreach (char c in potionsCollected) {
      switch (c) {
        case 'r':
          numReds++;
          break;
        case 'g':
          numGreens++;
          break;
        case 'b':
          numBlues++;
          break;
      }
    }

    Color newCollectedColor = new Color (numReds * colorNormalizer, numGreens * colorNormalizer, numBlues * colorNormalizer);
    collectedColor = newCollectedColor;
    collectedColorSprite.color = collectedColor;
    splashSystem.startColor = newCollectedColor;
  }

  private void UpdateAccuracy ()
  {
    vatAccuracy = numAccuratePotionsCollected / (double)potionsCollected.Count * 100;
    vatAccuracyText = vatAccuracy.ToString ("F1") + "%";
  }

  private void CheckIfFull ()
  {
    if (potionsCollected.Count >= targetVolume) {
      ProcessVat ();
    }
  }

  private void ProcessVat ()
  {
    sceneManager.ProcessVat (potionsCollected.Count, numAccuratePotionsCollected, requestColorString);
    Initialize ();
    sceneManager.UpdateTotalRequests ();
  }

  private void Initialize ()
  {
    targetVolume = UnityEngine.Random.Range (volumeMin, volumeMax + 1);
    potionRequests = CreatePotionRequests ();
    vatAccuracy = 0;
    vatAccuracyText = "100.0%";
    numAccuratePotionsCollected = 0;
    potionsCollected.Clear ();

    targetColor = GetTargetColor ();
    targetColorSprite.color = targetColor;

    collectedColor = Color.clear;
    collectedColorSprite.color = collectedColor;

    currentCollectedColorSize = 0;
    collectedColorTran.localScale = new Vector3 (4.6f, currentCollectedColorSize, 0);
  }

  private Color GetTargetColor ()
  {
    int numReds = 0;
    int numGreens = 0;
    int numBlues = 0;
    float colorNormalizer = 2.0f / targetVolume; //helps avoid oversaturation and black and white target color
    foreach (char c in potionRequests) {
      switch (c) {
        case 'r':
          numReds++;
          break;
        case 'g':
          numGreens++;
          break;
        case 'b':
          numBlues++;
          break;
      }
    }

    Color returnColor = new Color (numReds * colorNormalizer, numGreens * colorNormalizer, numBlues * colorNormalizer);
    return returnColor;
  }

  public void UpdateColorPool (int totPotDropped)
  {
    Debug.Log ("UpdateColorPool: " + totPotDropped);
    if (tier2Colors.Count > 0 && totPotDropped <= 100) {
      int tier2ColIndex = UnityEngine.Random.Range (0, tier2Colors.Count);
      string colorToAdd = tier2Colors [tier2ColIndex];
      colorPool.Add (colorToAdd);
      tier2Colors.Remove (colorToAdd);
    }

  }



}