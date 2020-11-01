using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipBlast : MonoBehaviour
{
    public SceneController sc;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (PlayerPrefs.GetInt("tooltip1") == 0)
            {
                sc.ProjTooltip();

            }
            PlayerPrefs.SetInt("tooltip1", 1);
            Destroy(this.gameObject);
        }
    }
}
