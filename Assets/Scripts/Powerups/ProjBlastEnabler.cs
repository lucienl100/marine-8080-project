using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjBlastEnabler : MonoBehaviour
{
    Transform t;
    private bool goingUp;
    public float ascensionSpeed = 0.5f;
    private Vector3 upper;
    private Vector3 bottom;
    private float height = 0.5f;
    public SceneController sc;

    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
        goingUp = true;
        bottom = t.position;
        upper = new Vector3(t.position.x, t.position.y + height, t.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        //Float movement
        if (goingUp)
        {
            t.position = Vector3.Slerp(t.position, upper, Time.deltaTime * ascensionSpeed);

            if (t.position.y >= upper.y - 0.1f)
            {
                goingUp = !goingUp;
            }
        }
        else
        {
            t.position = Vector3.Slerp(t.position, bottom, Time.deltaTime * ascensionSpeed);
            if (t.position.y <= bottom.y + 0.1f)
            {
                goingUp = !goingUp;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //Enable projectile blast ability
            AbilityManager am = other.gameObject.GetComponent<AbilityManager>();
            
            am.EnableProjBlast();
            //Activate tooltip if seen or touched for the first time
            if (PlayerPrefs.GetInt("tooltip1") == 0)
            {
                sc.ProjTooltip();
            }
            PlayerPrefs.SetInt("tooltip1", 1);
            Destroy(this.gameObject);
        }
    }
}
