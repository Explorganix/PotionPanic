﻿using UnityEngine;
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
    public AccuracyGauge ag;
    public SessionManager sessionManager;
    public RectTransform panelRectTransform;
    public Canvas canvas;
    public Text scoreBoard;
    public int score;
    public int displayScore;
    public int basePointsPerPotion;
    public List<string> tier1Colors = new List<string>() { "red", "green", "blue" };
    public List<string> tier2Colors = new List<string>() { "yellow", "pink", "sky" };

    // Use this for initialization
    void Awake()
    {
        baseDropsPerSecond = .2f;
        progressLevel = 1f;
        progressSpeed = .005f;
        dropTimer = 0;
        totalPotionsCollected = 0;
        overallAccuracy = 100;
        overallAccuracyText = "100.0%";
        activePotions = new List<char>();
        score = 0;
        displayScore = 0;
        totalPotionsDropped = 0;
        basePointsPerPotion = 50;
    }
    void Start()
    {
        sessionManager = GameObject.FindGameObjectWithTag("SessionManager").GetComponent<SessionManager>();
        difficultyLevel = sessionManager.GetDifficultyLevel();
        difficultyMultiplier = 1 + difficultyLevel / 20;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        UpdateTotalRequests();
        panelRectTransform = GameObject.FindGameObjectWithTag("Pause Panel").GetComponent<RectTransform>();
        scoreBoard = GameObject.FindGameObjectWithTag("High Score").GetComponent<Text>();
        scoreBoard.text = "$" + displayScore;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        dropTimer += activeDropsPerSecond * Time.deltaTime;
        if (totalPotionRequests.Count > 0)
        {
            if (dropTimer >= 1)
            {
                dropTimer = 0;
                DropPotion();
                UpdateProgress();
                totalPotionsDropped++;
                IncreaseScore(200);
            }
        }
    }

    private void UpdateProgress()
    {
        progressLevel += progressSpeed;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        switch (totalPotionsDropped)
        {
            case 20:
                leftVat.UpdateColorPool(totalPotionsDropped);
                midVat.UpdateColorPool(totalPotionsDropped);
                rightVat.UpdateColorPool(totalPotionsDropped);
                break;
            case 40:
                leftVat.UpdateColorPool(totalPotionsDropped);
                midVat.UpdateColorPool(totalPotionsDropped);
                rightVat.UpdateColorPool(totalPotionsDropped);
                break;
            case 60:
                leftVat.UpdateColorPool(totalPotionsDropped);
                midVat.UpdateColorPool(totalPotionsDropped);
                rightVat.UpdateColorPool(totalPotionsDropped);
                break;
        }
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

    public void ProcessVat(int vatBatchVolume, int numAccurateCollected, string requestColorString)
    {
        totalPotionsCollected += vatBatchVolume;
        totalAccuratePotionsCollected += numAccurateCollected;
        UpdateAccuracy(totalPotionsCollected, totalAccuratePotionsCollected);
        int colorTierMultiplier = 1;
        if (tier2Colors.Contains(requestColorString))
        {
            colorTierMultiplier = 2;
        }
        IncreaseScore(numAccurateCollected * basePointsPerPotion * colorTierMultiplier);
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
        foreach (char c in activePotions)
        {
            totalPotionRequests.Remove(c);
        }
    }

    public double GetAccuracy()
    {
        return overallAccuracy;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        panelRectTransform.SetParent(canvas.transform);
        panelRectTransform.GetComponentInChildren<ResizablePanel>().Grow();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        panelRectTransform.SetParent(this.transform);
    }

    public void IncreaseScore(int points)
    {
        score += points;
        StartCoroutine("IncrementScoreBoard");
    }
    IEnumerator IncrementScoreBoard()
    {
        while (displayScore < score)
        {
            displayScore += 5;

            if (displayScore % 40 == 0)
            {
                scoreBoard.fontSize -= 19;
            }
            if (displayScore % 20 == 0)
            {
                scoreBoard.fontSize = Mathf.Clamp(scoreBoard.fontSize + 10, 50, 109);
            }
            scoreBoard.text = "$" + displayScore;
            yield return null;
        }

    }
}