using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    private bool shieldEnabled = false;
    private bool shieldActive = false;
    public bool[] enabledAbilities = new bool[] { false, false };
    public float shieldDuration = 3f;
    private float shieldTimer;
    public GameObject shield;
    ProjectileBlast pb;
    GameObject activeShield;
    Transform player;
    public float cooldown = 10f;
    private float cdTimer;
    public GameObject shieldIcon;
    public GameObject projIcon;
    public GameObject shieldFlash;
    public GameObject projFlash;
    public Slider slider;
    public bool enableShieldAtStart = false;
    public bool enableBlastAtStart = false;
    // Start is called before the first frame update
    void Start()
    {
        shieldEnabled = false;
        pb = this.GetComponent<ProjectileBlast>();
        //If level was progressed from previous, enable unlocked abilities last level
        if (PlayerPrefs.GetInt("ability0") == 1)
        {
            Debug.Log("shield");
            EnableShield();
        }
        if (PlayerPrefs.GetInt("ability1") == 1)
        {
            EnableProjBlast();
        }
        cdTimer = 0f;
        shieldTimer = 0f;
        player = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldActive)
        {
            if (ShieldTimer())
            {
                shieldActive = false;
                DeactivateShield();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && shieldEnabled && cdTimer <= 0f)
        {
            //Enable shield
            Shield();
        }
        else if (cdTimer > 0f)
        {
            CooldownTick();
        }
        slider.value = Mathf.Max(cdTimer / cooldown, 0f);
    }
    public void EnableProjBlast()
    {
        //Method for enabling projectile blast and showing visuals
        projFlash.GetComponent<Flash>().FlashImage();
        enabledAbilities[1] = true;
        projIcon.SetActive(true);
        pb.enabled = true;
    }
    public void EnableShield()
    {
        //Method for enabling shield and showing visuals
        enabledAbilities[0] = true;
        shieldFlash.GetComponent<Flash>().FlashImage();
        shieldIcon.SetActive(true);
        shieldEnabled = true;
    }
    public void Shield()
    {
        //Method for activating shield
        cdTimer = cooldown;
        shieldTimer = shieldDuration;
        shieldActive = true;
        activeShield = Instantiate(shield, player.position, Quaternion.identity);
        activeShield.transform.parent = player;
    }
    void DeactivateShield()
    {
        Destroy(activeShield);
    }
    bool ShieldTimer()
    {
        if (shieldTimer <= 0f)
        {
            return true;
        }
        else
        {
            shieldTimer -= Time.deltaTime;
            return false;
        }
    }
    void CooldownTick()
    {
        cdTimer -= Time.deltaTime;
        if (cdTimer <= 0f)
        {
            shieldFlash.GetComponent<Flash>().FlashImage();
        }
    }
}
