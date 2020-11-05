using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHp : MonoBehaviour
{
    public SceneController sc;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (PlayerPrefs.GetInt("tooltip2") == 0)
            {
                sc.HealthpackTooltip();

            }
            PlayerPrefs.SetInt("tooltip2", 1);
            Destroy(this.gameObject);
        }
    }
}
