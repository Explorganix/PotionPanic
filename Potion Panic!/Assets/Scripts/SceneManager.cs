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

    public int colIndex;
    public char colChar;

    // Use this for initialization
    void Awake()
    {
        baseDropsPerSecond = .2f;
        progressLevel = 1f;
        difficultyLevel = 5f;
        difficultyMultiplier = 1 + difficultyLevel / 10;
        activeDropsPerSecond = baseDropsPerSecond * progressLevel * difficultyMultiplier;
        dropTimer = 0;
    }
	void Start () {
        totalPotionRequests = GetTotalPotionRequests();
	}
	
	// Update is called once per frame
	void Update () {
        dropTimer += activeDropsPerSecond * Time.deltaTime;
        if(dropTimer >= 1)
        {
            DropPotion();
            dropTimer = 0;
        }
	
	}

    void DropPotion()
    {
        GameObject newPotion = Instantiate(potionPrefab);
        colIndex = Random.Range(0, totalPotionRequests.Count);
        colChar = totalPotionRequests[colIndex];
        newPotion.GetComponent<Potion>().SetColor(colChar);
    }

    private List<char> GetTotalPotionRequests()
    {
        List<char> returnList = new List<char>();
        returnList.AddRange(leftVat.GetPotionRequests());
        returnList.AddRange(midVat.GetPotionRequests());
        returnList.AddRange(rightVat.GetPotionRequests());

        return returnList;
    }
}
