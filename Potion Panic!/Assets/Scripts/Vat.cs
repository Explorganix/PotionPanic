using System;
using UnityEngine;
using System.Collections.Generic;


public class Vat : MonoBehaviour {

    public int targetVolume;
    public int volumeMax;
    public int volumeMin;
    public List<char> potionRequests;
    public List<char> potionsCollected;
    public SceneManager sm;
    public int numAccuratePotionsCollected;
    public double vatAccuracy;
    public string vatAccuracyText;
    public Color targetColor;
    public Color collectedColor;
    public SpriteRenderer targetColorSprite;
    public SpriteRenderer collectedColorSprite;

    void Awake()
    {
        volumeMax = 4;
        volumeMin = 2;
        numAccuratePotionsCollected = 0;
        potionsCollected = new List<char>();
        Initialize();
    }

	void Start () {
        sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    List<char> CreatePotionRequests()
    {
        List<char> returnList = new List<char>();
        for (int i = 0; i < targetVolume; i++)
        {
            int colNum = UnityEngine.Random.Range(0, 3);
            char col = 'r';
            switch (colNum) {
                case 0: col = 'g'; break;
                case 1: col = 'b'; break;
                    }
            returnList.Add(col);
        }
        return returnList;
    }

    public List<char> GetPotionRequests()
    {
        return potionRequests.GetRange(0, potionRequests.Count);
    }

    public void ProcessPotion(char colorChar)
    {
        potionsCollected.Add(colorChar);
        UpdateColorCollected();
        if (potionRequests.Contains(colorChar))
        {
            potionRequests.Remove(colorChar);
            numAccuratePotionsCollected++;
        }
        else
        {
            sm.AddPotionRequest(colorChar);
        }
        UpdateAccuracy();
        CheckIfFull();
        Debug.Log("Potion being processed color is :" + colorChar);
    }

    private void UpdateColorCollected()
    {
        int numReds = 0;
        int numGreens = 0;
        int numBlues = 0;
        float colorNormalizer = 2.0f / potionsCollected.Count; //ensures that the collected color has accurate proportions of the potions collected thus far
        foreach (char c in potionsCollected)
        {
            switch (c)
            {
                case 'r': numReds++; break;
                case 'g': numGreens++; break;
                case 'b': numBlues++; break;
            }
        }

        Color newCollectedColor = new Color(numReds * colorNormalizer, numGreens * colorNormalizer, numBlues * colorNormalizer);
        collectedColor = newCollectedColor;
        collectedColorSprite.color = collectedColor;
    }

    private void UpdateAccuracy()
    {
        vatAccuracy = numAccuratePotionsCollected / (double)potionsCollected.Count * 100;
        vatAccuracyText = vatAccuracy.ToString("F1") + "%";
    }

    private void CheckIfFull()
    {
        if(potionsCollected.Count >= targetVolume)
        {
            ProcessVat();
        }
    }

    private void ProcessVat()
    {
        sm.ProcessVat(potionsCollected.Count, numAccuratePotionsCollected);
        Initialize();
        sm.UpdateTotalRequests();
    }

    private void Initialize()
    {
        targetVolume = UnityEngine.Random.Range(volumeMin, volumeMax + 1);
        potionRequests = CreatePotionRequests();
        vatAccuracy = 0;
        vatAccuracyText = "100.0%";
        numAccuratePotionsCollected = 0;
        potionsCollected.Clear();

        targetColor = GetTargetColor();
        targetColorSprite.color = targetColor;

        collectedColor = Color.clear;
        collectedColorSprite.color = collectedColor;
    }

    private Color GetTargetColor()
    {
        int numReds = 0;
        int numGreens = 0;
        int numBlues = 0;
        float colorNormalizer = 2.0f / targetVolume; //makes the sum of all rgb values equal to 255 to avoid oversaturation and black and white target color
        foreach (char c in potionRequests)
        {
            switch (c)
            {
                case 'r': numReds++; break;
                case 'g': numGreens++; break;
                case 'b': numBlues++; break;
            }
        }

        Color returnColor = new Color(numReds * colorNormalizer, numGreens * colorNormalizer, numBlues * colorNormalizer);
        return returnColor;
    }


}
