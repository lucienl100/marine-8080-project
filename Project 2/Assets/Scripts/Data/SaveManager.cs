using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void UnlockNextLevel(int currentLevel)
    {
        PlayerPrefs.SetInt("highestLevel", currentLevel + 1);
        Debug.Log("Level clear!");
        Debug.Log("Now all levels up to " + currentLevel + " are avaliable.");
    }

    public void DeleteData()
    {
        PlayerPrefs.SetInt("highestLevel", 0);
        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.SetFloat("volume", 1);
        PlayerPrefs.DeleteKey("tooltip0");
        PlayerPrefs.DeleteKey("tooltip1");
        for (int i = 1; i < 5; i++)
        {
            PlayerPrefs.DeleteKey("level" + i.ToString());
        }
        Debug.Log("Reseted all data");
    }
}
