using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public GameObject nocontinue;
    public GameObject set;
    public GameObject withcontinue;
    public AudioMixer am;
    void Start()
    {
        am.SetFloat("Volume", PlayerPrefs.GetFloat("volume", -5f));
        if (PlayerPrefs.GetInt("firstStartUp", 1) == 1)
        {
            PlayerPrefs.SetInt("firstStartUp", 0);
            for (int i = 1; i < 4; i++)
            {
                PlayerPrefs.SetInt("guns" + i.ToString(), 0);
            }
            for (int i = 0; i < 2; i++)
            {
                PlayerPrefs.SetInt("ability" + i.ToString(), 0);
            }
            for (int i = 1; i < 6; i++)
            {
                PlayerPrefs.SetInt("level" + i.ToString(), 0);
            }
            PlayerPrefs.SetInt("currentLevel", 0);
            PlayerPrefs.SetInt("highestLevel", 0);
            PlayerPrefs.SetFloat("volume", -5f);
        }
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
        for (int i = 0; i < 2; i++)
        {
            PlayerPrefs.SetInt("tooltip" + i.ToString(), 0);
            PlayerPrefs.SetInt("ability" + i.ToString(), 0);
        }
        PlayerPrefs.SetInt("tooltip2", 0);
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
