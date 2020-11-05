using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{
    public Canvas mm;
    public Canvas set;
    
    public void Back()
    {
        mm.enabled = true;
        set.enabled = false;
    }
}
