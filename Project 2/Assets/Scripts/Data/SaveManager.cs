using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void UnlockNextLevel(int currentLevel)
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        Debug.Log("Level clear!");
        Debug.Log("Now all levels up to " + currentLevel + " are avaliable.");
    }

    public void DeleteData()
    {
        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.SetFloat("volume", 1);
        Debug.Log("Reset all data");
    }
}
