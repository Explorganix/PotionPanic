using UnityEngine;
using System.Collections;
using System;

public class SessionManager : MonoBehaviour {

    public static SessionManager instance = null;

    public int difficultyLevel;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public int GetDifficultyLevel()
    {
        return difficultyLevel;
            }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetDifficultyLevel(int lvl)
    {
        difficultyLevel = lvl;
    }

    public void LoadLevel(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
