using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator anim;
    public GameObject pauseMenuUI;
    public GameObject crosshair;
    private int currBuildIndex;
    private int nextBuildIndex;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        //Get current scene index and next scene index
        currBuildIndex = SceneManager.GetActiveScene().buildIndex;
        nextBuildIndex = currBuildIndex + 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        crosshair.SetActive(false);
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    // Update is called once per frame
    public void FadeToLevel()
    {
        anim.SetTrigger("FadeOut");
    }
    public void WinScreen()
    {
        if (nextBuildIndex >= PlayerPrefs.GetInt("highestLevel"))
        {
            //If the next level exceeds the players highest level, open up the next level in the level select.
            PlayerPrefs.SetInt("highestLevel", nextBuildIndex);
        }
        //Set the current playthrough level to the next level
        PlayerPrefs.SetInt("currentLevel", nextBuildIndex);
        StartCoroutine(LoadWinScreen());
    }
    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }
    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }
    IEnumerator LoadWinScreen()
    {
        FadeToLevel();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(4);
    }
    IEnumerator LoadNextLevel()
    {
        FadeToLevel();
        Debug.Log(PlayerPrefs.GetInt("currentLevel"));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
    }
    IEnumerator LoadMainMenu()
    {
        FadeToLevel();
        Debug.Log("loading main menu");
        yield return new WaitForSeconds(2f);
        Debug.Log("loading main menu");
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
