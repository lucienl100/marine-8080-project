using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expiry : MonoBehaviour
{
    public float timeLeft = Mathf.Infinity;
    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
        timeLeft -= Time.deltaTime;
    }
}
