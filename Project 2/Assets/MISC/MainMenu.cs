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
        SceneManager.LoadScene("Scenes/Level01");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
