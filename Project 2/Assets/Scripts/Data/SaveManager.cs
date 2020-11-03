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
        for (int i = 1; i < 6; i++)
        {
            PlayerPrefs.DeleteKey("level" + i.ToString());
        }
        for (int i = 0; i < 2; i++)
        {
            PlayerPrefs.SetInt("ability0", 0);
            PlayerPrefs.SetInt("ability1", 0);
        }
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        Debug.Log("Reseted all data");
    }
}
