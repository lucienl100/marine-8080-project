using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject nocontinue;
    public GameObject set;
    public GameObject withcontinue;
    void Start()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
    }
    public void PlayGame()
    {
        if (PlayerPrefs.GetInt("highestLevel") == 0 && PlayerPrefs.GetInt("currentLevel") == 0)
        {
            nocontinue.SetActive(true);
        }
        else
        {
            withcontinue.SetActive(true);
        }
    }
    public void NewGame()
    {
        for (int i = 1; i < 4; i++)
        {
            PlayerPrefs.SetInt("guns" + i.ToString(), 0);
        }
        PlayerPrefs.SetInt("currentLevel", 1);
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
    }
    public void Quit()
    {
        Application.Quit();
    }
}
