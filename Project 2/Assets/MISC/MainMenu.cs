using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mm;
    public GameObject set;
    public GameObject levelselect;
    public void PlayGame()
    {
        if (PlayerPrefs.GetInt("currentLevel") == 0)
        {
            SceneManager.LoadScene("Scenes/Level01");
        }
        else
        {
            LoadLevelSelect();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadLevelSelect()
    {
        levelselect.SetActive(true);
        mm.SetActive(false);
    }
}
