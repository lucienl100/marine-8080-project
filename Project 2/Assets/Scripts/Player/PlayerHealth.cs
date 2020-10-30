using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            Die();
        }
        healthText.text = ((int)health).ToString();
    }
    public void Damage(float damage)
    {
        Debug.Log("got hit");
        health -= damage;
    }
    void Die()
    {
        lam.enabled = false;
        s.enabled = false;
        mv.enabled = false;
        anim.SetTrigger("Die");
        Invoke("RestartScene", 3f);
    }
    void RestartScene()
    {
        SceneManager.LoadScene(8);
    }
}
