using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadLevel(string levelname)
    {
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        PlayerPrefs.SetInt("ability0", 0);
        PlayerPrefs.SetInt("ability1", 0);
        SceneManager.LoadScene(levelname);
    }
}
