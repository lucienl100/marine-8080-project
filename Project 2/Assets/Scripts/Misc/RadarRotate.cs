using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarRotate : MonoBehaviour
{
    Transform t;
    public float speed = 5f;
    public GameObject inst;
    public Transform dish;
    public Transform player;
    public bool completed = false;
    public bool inRange;
    private float range = 3f;
    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!completed)
        {
            dish.eulerAngles = new Vector3(dish.eulerAngles.x, dish.eulerAngles.y + speed * Time.deltaTime, dish.eulerAngles.z);
            if ((t.position - player.position).magnitude < range)
            {
                inst.SetActive(true);
                inRange = true;
            }
            else
            {
                inst.SetActive(false);
                inRange = false;
            }
        }
        
    }
}
