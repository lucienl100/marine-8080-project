﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    private float health;
    public Animator anim;
    public LookAtMouse lam;
    public Shooting s;
    public Movement mv;
    public float minHeight = -14f;
    public AudioSource hitsound;
    CharacterController cc;
    float difficulty;
    public HitFlash hf;
    // Start is called before the first frame update
    void Start()
    {
        difficulty = PlayerPrefs.GetFloat("difficulty");
        cc = this.GetComponent<CharacterController>();
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOutOfBounds();
        if (health <= 0f)
        {
            Die();
        }
        //Display the health
        healthText.text = ((int)health).ToString();
    }
    public void Damage(float damage)
    {
        //Method for taking damage
        health -= damage * difficulty;
        hitsound.Play();
        hf.FlashImage();
        if (health <= 0f)
        {
            health = 0f;
        }
    }
    void Die()
    {
        Time.timeScale = 0.34f;
        lam.enabled = false;
        s.enabled = false;
        mv.enabled = false;
        cc.Move(new Vector3(0f, -9.8f * Time.deltaTime, 0f));
        anim.SetTrigger("Die");
        Invoke("GameOver", 1f);
    }
    public void AddHealth(float add)
    {
        health += add;
        if (health > 100f)
        {
            health = 100f;
        }
    }
    void CheckOutOfBounds()
    {
        //Check if the player is too low in the level
        if (this.transform.position.y < minHeight)
        {
            Die();
        }
    }
    void GameOver()
    {
        //Load gameover scene
        SceneManager.LoadScene(9);
    }
}
