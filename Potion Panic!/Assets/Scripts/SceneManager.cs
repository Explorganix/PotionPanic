using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {

    public float baseDropsPerSecond;
    public float activeDropsPerSecond;
    public float difficultyLevel;
    public float difficultyMultiplier;
    public float progressLevel;
    public float dropTimer;
    public GameObject potionPrefab;
    public Vat leftVat;
    public Vat midVat;
    public Vat rightVat;
    public List<char> totalPotionRequests;
    public int totalPotionsCollected;
    public int totalAccuratePotionsCollected;
    public double overallAccuracy;
    public string overallAccuracyText;

    // Use this for initialization
    void Awake()
    {
        baseDropsPerSecond = .2f;
        progressLevel = 1f;
        difficultyLevel = 5f;
        difficultyMultiplier = 1 + difficultyLevel / 10;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        dropTimer = 0;
        totalPotionsCollected = 0;
        overallAccuracy = 0;
        overallAccuracyText = "100.0%";
    }
	void Start () {
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
                dropTimer = 0;
            }
        }	
	}

    void DropPotion()
    {
        GameObject newPotion = Instantiate(potionPrefab);
        int colIndex = UnityEngine.Random.Range(0, totalPotionRequests.Count);
        char colChar = totalPotionRequests[colIndex];
        newPotion.GetComponent<Potion>().SetColor(colChar);
        totalPotionRequests.Remove(colChar);
    }

    internal void AddPotionRequest(char colorChar)
    {
        totalPotionRequests.Add(colorChar);
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
    }

    public void UpdateTotalRequests()
    {
        totalPotionRequests = GetTotalPotionRequests();
    }

}
