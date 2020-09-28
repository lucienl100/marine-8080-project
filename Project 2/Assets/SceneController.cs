using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator anim;
    private int currBuildIndex;
    private int nextBuildIndex;
    // Start is called before the first frame update
    void Start()
    {
        currBuildIndex = SceneManager.GetActiveScene().buildIndex;
        nextBuildIndex = currBuildIndex + 1;
    }
    // Update is called once per frame
    public void FadeToLevel()
    {
        anim.SetTrigger("FadeOut");
    }
    public void LoadNextScene(int currentLevel)
    {
        if (currentLevel >= PlayerPrefs.GetInt("currentLevel"))
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
        StartCoroutine(LoadNextLevel(nextBuildIndex));
    }
    IEnumerator LoadNextLevel(int levelIndex)
    {
        FadeToLevel();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextBuildIndex);
    }
}
