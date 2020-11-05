using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    Transform t;
    private bool goingUp;
    public float ascensionSpeed = 0.5f;
    private Vector3 upper;
    private Vector3 bottom;
    private float height = 0.5f;
    public SceneController sc;
    public float healAmount = 25f;

    public GameObject PE;
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
            PlayerHealth am = other.gameObject.GetComponent<PlayerHealth>();
            //Heal player
            am.AddHealth(healAmount);
            if (PlayerPrefs.GetInt("tooltip2") == 0)
            {
                //Activate tooltip if touched or seen for the first time
                sc.ShieldTooltip();

            }
            Instantiate(PE, this.transform.position, Quaternion.identity);
            PlayerPrefs.SetInt("tooltip2", 1);
            Destroy(this.gameObject);
        }
    }
}
