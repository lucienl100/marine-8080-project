using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileBlast : MonoBehaviour
{
    Transform t;
    public float radius = 30;
    public ParticleSystem blast;
    public GameObject projFlash;
    public float cooldown = 15f;
    float timer;
    public Slider slider;
    void Start()
    {
        timer = 0f;
    }
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F) && timer <= 0f)
		{
            DestoryProjectiles();
            timer = cooldown;
		}
        else if (timer > 0f)
        {
            CDTick();
        }
        slider.value = Mathf.Max(timer / cooldown, 0f);
    }

    void DestoryProjectiles()
	{
        blast.Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in colliders)
		{
            if(hitCollider.gameObject.GetComponent<IProjectile>() != null)
			{
                Destroy(hitCollider.gameObject);
			}
		}
	}
    void CDTick()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            projFlash.GetComponent<Flash>().FlashImage();
        }
    }
}
