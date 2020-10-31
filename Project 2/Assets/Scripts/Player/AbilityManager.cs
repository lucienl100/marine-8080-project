using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    private bool shieldEnabled = false;
    private bool shieldActive = false;
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
        if (enableShieldAtStart)
        {
            EnableShield();
        }
        if (enableBlastAtStart)
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
        projIcon.SetActive(true);
        pb.enabled = true;
    }
    public void EnableShield()
    {
        shieldFlash.GetComponent<Flash>().FlashImage();
        shieldIcon.SetActive(true);
        shieldEnabled = true;
    }
    public void Shield()
    {
        cdTimer = cooldown;
        shieldTimer = shieldDuration;
        shieldActive = true;
        activeShield = Instantiate(shield, player.position, Quaternion.identity);
        activeShield.transform.parent = player;
        Debug.Log("shield");
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
