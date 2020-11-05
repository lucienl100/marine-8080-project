using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Image flashImage;
    private float flashSpeed = 1.8f;
    private Color flashColour = new Color(1f, 1f, 1f, 2f);
    public bool flash;
    Vector2 flashSize = new Vector2(450f, 450f);
    private Color originalColor = new Color(0.5f, 0.5f, 0.5f, 0f);
    void Update()
    {
        if (flash)
        {
            //Increase color alpha and size of flash image
            flashImage.color = flashColour;
            flashImage.rectTransform.sizeDelta = flashSize;
        }
        else
        {
            //Quickly interpolate the size and alpha back to normal
            flashImage.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(flashImage.rectTransform.sizeDelta.x, 40f, flashSpeed * Time.deltaTime), Mathf.Lerp(flashImage.rectTransform.sizeDelta.y, 40f, flashSpeed * Time.deltaTime));
            flashImage.color = Color.Lerp(flashImage.color, originalColor, flashSpeed * Time.deltaTime);
        }
        flash = false;
    }
    public void FlashImage()
    {
        flash = true;
    }
}