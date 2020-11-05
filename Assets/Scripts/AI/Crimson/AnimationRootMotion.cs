using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRootMotion : MonoBehaviour
{
    public Attacks a;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorMove()
    {
        if (a.sweep)
        {
            Debug.Log("turning");
            this.transform.eulerAngles += new Vector3(0f, 360f / 2.5f * Time.deltaTime, 0f);
        }
        else
        {
            //this.transform.localRotation = Quaternion.identity;
        }
    }
}
