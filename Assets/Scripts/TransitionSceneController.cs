﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionSceneController : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
    }
    public Animator anim;
    public void FadeToLevel()
    {
        anim.SetTrigger("FadeOut");
    }
    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }
    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }
    IEnumerator LoadNextLevel()
    {
        FadeToLevel();
        Debug.Log(PlayerPrefs.GetInt("currentLevel"));
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
    }
    IEnumerator LoadMainMenu()
    {
        FadeToLevel();
        Debug.Log("loading main menu");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("loading main menu");
        SceneManager.LoadScene(0);
    }
}
