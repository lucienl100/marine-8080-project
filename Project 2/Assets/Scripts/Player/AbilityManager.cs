using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private bool shieldEnabled = false;
    private bool shieldActive = false;
    public float shieldDuration = 3f;
    private float shieldTimer;
    public GameObject shield;
    public GameObject activeShield;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
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
        else if (Input.GetKeyDown(KeyCode.Q) && shieldEnabled)
        {
            Shield();
        }
    }
    public void EnableShield()
    {
        shieldEnabled = true;
    }
    public void Shield()
    {
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
}
