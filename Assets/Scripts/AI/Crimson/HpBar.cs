using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider hp;
    public HealthSystemBoss bossHealth;
    void Start()
    {
        hp = this.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        hp.value = 1 - (bossHealth.currentHp <= 0 ? 0 : (bossHealth.currentHp / bossHealth.maxHp));
    }
}
