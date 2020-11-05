using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * Time.deltaTime*speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * Time.deltaTime*speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= Vector3.right * Time.deltaTime*speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= Vector3.up * Time.deltaTime*speed;
        }
    }
}
