using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadLevel(string levelname)
    {
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        SceneManager.LoadScene(levelname);
    }
}
