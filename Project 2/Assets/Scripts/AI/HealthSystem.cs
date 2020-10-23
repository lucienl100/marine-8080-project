using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHp = 100f;
    private float currentHp;
    public SkinnedMeshRenderer meshRenderer;
    public Animator anim;
    LookAtPlayer lap;
    public ShootingBasic sb;
    Activate a;
    Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = meshRenderer.material.color;
        currentHp = maxHp;
        lap = this.GetComponent<LookAtPlayer>();
        a = this.GetComponent<Activate>();
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
        Flash();
        Invoke("ResetColor", 0.1f);
    }
    void CheckHealth()
    {
        if (currentHp <= 0f)
        {
            anim.SetTrigger("Die");
            Debug.Log("destroyed");
            a.enabled = false;
            lap.enabled = false;
            sb.enabled = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            Decay();
        }
    }
    void Flash()
    {
        meshRenderer.material.color = Color.red;
    }
    void ResetColor()
    {
        meshRenderer.material.color = originalColor;
    }
    void Decay()
    {
        Destroy(this.gameObject, 3f);
    }
}
