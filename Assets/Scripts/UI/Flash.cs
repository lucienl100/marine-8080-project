using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Image flashImage;
    public float flashSpeed = 5f;
    //Time the flash lasts for
    public Color flashColour = new Color(1f, 1f, 1f, 2f);
    public bool flash;
    // The values above correspond to: R - G - B - Alpha these values would produce an opaque white flash.

    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        if (flash)
        {
            flashImage.color = flashColour;
            flashImage.rectTransform.sizeDelta = new Vector2(200f, 200f);
        }
        else
        {
            flashImage.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(flashImage.rectTransform.sizeDelta.x, 40f, flashSpeed * Time.deltaTime), Mathf.Lerp(flashImage.rectTransform.sizeDelta.y, 40f, flashSpeed * Time.deltaTime));
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        flash = false;
    }
    public void FlashImage()
    {
        flash = true;
    }
}