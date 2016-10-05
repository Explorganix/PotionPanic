using UnityEngine;
using System.Collections.Generic;
using System;

public class SceneManager : MonoBehaviour {

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
    public double overallAccuracy;
    public string overallAccuracyText;
    public AccuracyGauge ag;
    public SessionManager sessionManager;

    // Use this for initialization
    void Awake()
    {
        baseDropsPerSecond = .2f;
        progressLevel = 1f;
        progressSpeed = .025f;
        dropTimer = 0;
        totalPotionsCollected = 0;
        overallAccuracy = 100;
        overallAccuracyText = "100.0%";
        activePotions = new List<char>();
    }
	void Start () {
        sessionManager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        difficultyLevel = sessionManager.GetDifficultyLevel();
        difficultyMultiplier = 1 + difficultyLevel / 20;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        UpdateTotalRequests();
	}
	
	// Update is called once per frame
	void Update () {
        dropTimer += activeDropsPerSecond * Time.deltaTime;
        if(totalPotionRequests.Count > 0)
        {
            if (dropTimer >= 1)
            {
                DropPotion();
                UpdateProgress();
                dropTimer = 0;
            }
        }	
	}

    private void UpdateProgress()
    {
        progressLevel += progressSpeed;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        leftVat.UpdateProgress(progressLevel);
        midVat.UpdateProgress(progressLevel);
        rightVat.UpdateProgress(progressLevel);
    }

    void DropPotion()
    {
        GameObject newPotion = Instantiate(potionPrefab);
        int colIndex = UnityEngine.Random.Range(0, totalPotionRequests.Count);
        char colChar = totalPotionRequests[colIndex];
        newPotion.GetComponent<Potion>().SetColor(colChar);
        totalPotionRequests.Remove(colChar);
        activePotions.Add(colChar);
    }

    internal void AddPotionRequest(char colorChar)
    {
        totalPotionRequests.Add(colorChar);
    }

    public void RemoveActivePotion(char colorChar)
    {
        activePotions.Remove(colorChar);
    }

    private List<char> GetTotalPotionRequests()
    {
        List<char> returnList = new List<char>();
        returnList.AddRange(leftVat.GetPotionRequests());
        returnList.AddRange(midVat.GetPotionRequests());
        returnList.AddRange(rightVat.GetPotionRequests());

        return returnList;
    }

    public void ProcessVat(int vatBatchVolume, int numAccurateCollected)
    {
        totalPotionsCollected += vatBatchVolume;
        totalAccuratePotionsCollected += numAccurateCollected;
        UpdateAccuracy(totalPotionsCollected, totalAccuratePotionsCollected);
    }

    private void UpdateAccuracy(int tpc, int tacp)
    {
        overallAccuracy = tacp / (double)tpc * 100;
        overallAccuracyText = overallAccuracy.ToString("F1") + "%";
        ag.SetAccuracy();
    }

    public void UpdateTotalRequests()
    {
        totalPotionRequests = GetTotalPotionRequests();
        foreach(char c in activePotions)
        {
            totalPotionRequests.Remove(c);
        }
    }

    public double GetAccuracy()
    {
        return overallAccuracy;
    }

}
