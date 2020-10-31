﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SceneController : MonoBehaviour
{
    public Animator anim;
    public AudioMixer am;
    public Slider slider;
    public GameObject pauseMenuUI;
    public Shooting shooting;
    public GameObject crosshair;
    public GameObject hud;
    private int currBuildIndex;
    private int nextBuildIndex;
    public LookAtMouse lam;
    public Shooting shoot;
    public GameObject fade;
    public Movement mv;
    public PlayerHealth ph;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        float value;
        am.GetFloat("Volume", out value);
        slider.value = value;
        Cursor.visible = false;
        //Get current scene index and next scene index
        currBuildIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("currentLevel", currBuildIndex);
        nextBuildIndex = currBuildIndex + 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PauseGame();
            paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            UnpauseGame();
        }
        am.SetFloat("Volume", slider.value);
    }
    public void PauseGame()
    {
        crosshair.SetActive(false);
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        hud.SetActive(false);
        shoot.enabled = false;
        lam.enabled = false;
    }
    public void UnpauseGame()
    {
        paused = false;
        crosshair.SetActive(true);
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        hud.SetActive(true);
        shoot.enabled = true;
        lam.enabled = true;
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
        bool[] obtainedguns = shooting.enabledguns;
        for (int i = 0; i < 4; i++)
        {
            if (obtainedguns[i])
            {
                PlayerPrefs.SetInt("guns" + i.ToString(), 1);
            }
            else
            {
                PlayerPrefs.SetInt("guns" + i.ToString(), 0);
            }
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
        fade.SetActive(true);
        ph.enabled = false;
        mv.enabled = false;
        FadeToLevel();
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TransitionScene");
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
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene(0);
    }
}
