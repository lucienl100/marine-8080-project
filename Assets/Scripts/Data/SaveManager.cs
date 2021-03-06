﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public float numLevels = 6;
    public void UnlockNextLevel(int currentLevel)
    {
        PlayerPrefs.SetInt("highestLevel", currentLevel + 1);
        Debug.Log("Level clear!");
        Debug.Log("Now all levels up to " + currentLevel + " are avaliable.");
    }

    public void DeleteData()
    {
        //Deletes all the player preferences, where the data is stored
        PlayerPrefs.SetInt("highestLevel", 0);
        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.SetFloat("volume", 1);
        PlayerPrefs.DeleteKey("tooltip0");
        PlayerPrefs.DeleteKey("tooltip1");
        PlayerPrefs.DeleteKey("tooltip2");
        PlayerPrefs.SetInt("ability0", 0);
        PlayerPrefs.SetInt("ability1", 0);
        for (int i = 1; i < numLevels; i++)
        {
            PlayerPrefs.SetInt("level" + i.ToString(), 0);
        }
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        PlayerPrefs.DeleteAll();
        Debug.Log("Reseted all data");
    }
}
