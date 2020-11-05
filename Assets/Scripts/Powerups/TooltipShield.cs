using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipShield : MonoBehaviour
{
    public SceneController sc;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (PlayerPrefs.GetInt("tooltip0") == 0)
            {
                sc.ShieldTooltip();

            }
            PlayerPrefs.SetInt("tooltip0", 1);
            Destroy(this.gameObject);
        }
    }
}
