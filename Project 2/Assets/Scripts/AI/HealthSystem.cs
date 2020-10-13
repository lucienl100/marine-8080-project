using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHp = 100f;
    private float currentHp;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
    public void Damage(float damage)
    {
        Debug.Log("took damage");
        currentHp -= damage;
    }
    void CheckHealth()
    {
        if (currentHp <= 0f)
        {
            Debug.Log("destroyed");
            Destroy(this.gameObject);
        }
    }
}
