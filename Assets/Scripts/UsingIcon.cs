using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingIcon : MonoBehaviour
{
    RectTransform rect;
    public Shooting shooting;
    public float anchorPos = -335f;
    public float iconDistance = 45f;
    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (shooting.guns[i])
            {
                //Change using icon position depending on what gun is being used
                rect.localPosition = new Vector3(anchorPos+ i * iconDistance, rect.localPosition.y, 0f);
            }
        }
    }
}
