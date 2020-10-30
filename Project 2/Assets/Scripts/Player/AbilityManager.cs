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
    public GameObject image;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        pb = this.GetComponent<ProjectileBlast>();
        cdTimer = 0f;
        shieldTimer = 0f;
        player = this.transform;
        shieldEnabled = false;
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
        pb.enabled = true;
    }
    public void EnableShield()
    {
        image.SetActive(true);
        shieldEnabled = true;
    }
    public void Shield()
    {
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
    }
}
