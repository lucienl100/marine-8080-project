using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitFlash : MonoBehaviour
{
    Image flashImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 1f, 1f, 0.55f);
    public bool flash;
    void Start()
    {
        flashImage = this.GetComponent<Image>();
    }
    void Update()
    {
        if (flash)
        {
            flashImage.color = flashColour;
            flash = false;
        }
        else
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        
    }
    public void FlashImage()
    {
        Debug.Log("FLASH");
        flash = true;
    }
}