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
    ShootingBasic sb;
    ShootingSpread sp;
    Activate a;
    Color originalColor;
    public int type;
    float fAtt;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = meshRenderer.material.GetVector("_SurfaceColor");
        fAtt = meshRenderer.material.GetFloat("_Ka");
        currentHp = maxHp;
        lap = this.GetComponent<LookAtPlayer>();
        a = this.GetComponent<Activate>();
        if (type == 0)
        {
            sb = this.GetComponent<ShootingBasic>();
        }
        if (type == 1)
        {
            sp = this.GetComponent<ShootingSpread>();
        }
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
            if (type == 0)
            {
                sb.enabled = false;
            }
            if (type == 1)
            {
                sp.enabled = false;
            }
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            Decay();
        }
    }
    void Flash()
    {
        meshRenderer.material.SetVector("_SurfaceColor", Color.red);
        meshRenderer.material.SetFloat("_Ka", 4);
    }
    void ResetColor()
    {
        meshRenderer.material.SetVector("_SurfaceColor", originalColor);
        meshRenderer.material.SetFloat("_Ka", fAtt);
    }
    void Decay()
    {
        Destroy(this.gameObject, 3f);
    }
}
