﻿using UnityEngine;
using System.Collections.Generic;

public class Vat : MonoBehaviour {

    public int targetVolume;
    public int volumeMax;
    public int volumeMin;
    public List<char> potionRequests;

    void Awake()
    {
        volumeMax = 4;
        volumeMin = 2;
        targetVolume = Random.Range(volumeMin, volumeMax + 1);
        potionRequests = CreatePotionRequests(targetVolume);
    }
	// Use this for initialization
	void Start () {
	    for(int i = 0; i < targetVolume; i++)
        {
            potionRequests.Add('r');
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    List<char> CreatePotionRequests(int tv)
    {
        List<char> returnList = new List<char>();
        for (int i = 0; i < tv; i++)
        {
            int colNum = Random.Range(0, 2);
            char col = 'r';
            switch (colNum) {
                case 0: col = 'g'; break;
                case 1: col = 'b'; break;
                    }
            potionRequests.Add(col);
        }
        return returnList;
    }

    public List<char> GetPotionRequests()
    {
        return potionRequests.GetRange(0, potionRequests.Count);
    }
}