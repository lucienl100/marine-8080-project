using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas mm;
    public Canvas set;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level01", LoadSceneMode.Additive);
    }
    public void Settings()
    {
        mm.enabled = false;
        set.enabled = true;
    }
}
