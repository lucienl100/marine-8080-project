using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRotate : MonoBehaviour
{
    Transform t;
    public float speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y + speed * Time.deltaTime, t.eulerAngles.z);
    }
}
