using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtMouse : MonoBehaviour
{
    // Update is called once per frame
    Vector3 mousePosition;
    public Vector3 mouseWorld;
    public bool playerIsRight = false;
    Quaternion faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    public Camera cam;
    private float camdepth = 15f;
    public Animator anim;
    Transform t;
    public Transform crosshair;
    public Transform weapon;
    public Transform chest;
    void Start()
    {
        t = this.transform;
    }
    void Update()
    {
        
        mousePosition = Input.mousePosition;
        mousePosition.z = camdepth;
        crosshair.position = mousePosition;
        
        mouseWorld = cam.ScreenToWorldPoint(mousePosition);
        if (mouseWorld.x < t.position.x - 0.1f)
        {
            playerIsRight = true;
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        else if (mouseWorld.x > t.position.x + 0.1f)
        {
            playerIsRight = false;
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
        t.eulerAngles = new Vector3(0f, t.eulerAngles.y, 0f);
    }
    void LateUpdate()
    {
        Vector3 dirToLook = mouseWorld - t.position - new Vector3(0f, 0.1f, 0f);
        Quaternion rotation = Quaternion.LookRotation(dirToLook);
        chest.rotation = Quaternion.Euler(rotation.eulerAngles.x, chest.eulerAngles.y, rotation.eulerAngles.z);
    }
}

