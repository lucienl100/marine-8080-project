using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public Text text;
    public float difficulty = 1;
    // Start is called before the first frame update
    void Start()
    {
        difficulty = 1;
        PlayerPrefs.SetFloat("difficulty", (difficulty + 1) * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        DifficultyUpdate();
    }
    void DifficultyUpdate()
    {
        if (difficulty == 0)
        {
            text.text = "Difficulty: Easy";
        }
        else if (difficulty == 1)
        {
            text.text = "Difficulty: Normal";
        }
        else if (difficulty == 2)
        {
            text.text = "Difficulty: Hard";
        }
    }
    public void ChangeDifficulty()
    {
        difficulty = (difficulty + 1) % 3;
        PlayerPrefs.SetFloat("difficulty", (difficulty+1) * 0.5f);
    }
}
