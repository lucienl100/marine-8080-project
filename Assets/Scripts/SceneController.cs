using System.Collections;
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
    public GameObject shieldTooltip;
    public GameObject projTooltip;
    public GameObject hpTooltip;
    public GameObject hud;
    private int currBuildIndex;
    private int nextBuildIndex;
    public LookAtMouse lam;
    public Shooting shoot;
    public AbilityManager ability;
    public GameObject fade;
    public Movement mv;
    public PlayerHealth ph;
    public bool paused = false;
    bool tooltip = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        
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
        if (tooltip)
        {
            //Freeze game for tooltip
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                shieldTooltip.SetActive(false);
                projTooltip.SetActive(false);
                hpTooltip.SetActive(false);
                tooltip = false;
                UnpauseGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && paused)
            {
                UnpauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !paused) 
            {
                PauseGame();
            }
            am.SetFloat("Volume", slider.value);
        }
    }
    public void PauseGame()
    {
        //Freeze time and disable shooting and looking around
        paused = true;
        Debug.Log("paused");
        Time.timeScale = 0f;
        crosshair.SetActive(false);
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        
        hud.SetActive(false);
        shoot.enabled = false;
        lam.enabled = false;
    }
    public void UnpauseGame()
    {
        //Unfreeze time and enable player movement and combat
        paused = false;
        Time.timeScale = 1f;
        paused = false;
        crosshair.SetActive(true);
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        hud.SetActive(true);
        shoot.enabled = true;
        lam.enabled = true;
        
    }
    public void HealthpackTooltip()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        hud.SetActive(false);
        shoot.enabled = false;
        lam.enabled = false;
        tooltip = true;
        hpTooltip.SetActive(true);
    }
    public void ShieldTooltip()
    {
        //Display shield tooltip
        Cursor.visible = true;
        Time.timeScale = 0f;
        hud.SetActive(false);
        shoot.enabled = false;
        lam.enabled = false;
        tooltip = true;
        shieldTooltip.SetActive(true);
    }
    public void ProjTooltip()
    {
        //Display projectile blast tooltip
        Cursor.visible = true;
        Time.timeScale = 0f;
        hud.SetActive(false);
        shoot.enabled = false;
        lam.enabled = false;
        tooltip = true;
        projTooltip.SetActive(true);
    }
    public void FadeToLevel()
    {
        //Fade animation
        anim.SetTrigger("FadeOut");
    }
    public void WinScreen()
    {
        ph.enabled = false;
        mv.enabled = false;
        lam.enabled = false;
        bool[] obtainedguns = shooting.enabledguns;
        //Preserve the obtained guns for the next level
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
        //Preserve the obtained abilities for the next level
        for (int i = 0; i < ability.enabledAbilities.Length; i++)
        {
            if (ability.enabledAbilities[i])
            {
                PlayerPrefs.SetInt("ability" + i.ToString(), 1);
            }
            else
            {
                PlayerPrefs.SetInt("ability" + i.ToString(), 0);
            }
        }
        //Set the current playthrough level to the next level
        PlayerPrefs.SetInt("currentLevel", nextBuildIndex);
        PlayerPrefs.SetInt("level" + (nextBuildIndex - 1).ToString(), 1);
        StartCoroutine(LoadWinScreen());
    }
    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        fade.SetActive(true);
        FadeToLevel();
        ph.enabled = false;
        mv.enabled = false;
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
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
    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }
}
