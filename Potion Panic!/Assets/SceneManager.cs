using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public float baseDropsPerSecond;
    public float activeDropsPerSecond;
    public float difficultyLevel;
    public float difficultyMultiplier;
    public float progressLevel;
    public float dropTimer;

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
	
	}
	
	// Update is called once per frame
	void Update () {
        if(1 / activeDropsPerSecond >= 1)
        {
            DropPotion();
        }
	
	}

    void DropPotion()
    {
        //Instantiate()
    }
}
