using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelSelect : MonoBehaviour
{
    public Button[] levels;
    void Start()
    {
        for (int i = 1; i < levels.Length; i++)
        {
            if (PlayerPrefs.HasKey("level" + (i+1).ToString()))
            {
                levels[i].enabled = true;
            }
            else
            {
                levels[i].enabled = false;
            }
        }
    }
    // Start is called before the first frame update
    public void LoadLevel(string levelname)
    {
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        for (int i = 0; i < 2; i++)
        {
            PlayerPrefs.SetInt("ability" + i.ToString(), 0);
        }
        SceneManager.LoadScene(levelname);
    }
}
