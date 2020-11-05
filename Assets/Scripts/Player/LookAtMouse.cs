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
        //Grab mouse position on screen
        mousePosition = Input.mousePosition;
        mousePosition.z = camdepth;
        crosshair.position = mousePosition;
        
        //Get screen position of mouse to world position
        mouseWorld = cam.ScreenToWorldPoint(mousePosition);
        if (mouseWorld.x < t.position.x - 0.1f)
        {
            //Turn the player left
            playerIsRight = true;
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        else if (mouseWorld.x > t.position.x + 0.1f)
        {
            //Turn the player right
            playerIsRight = false;
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        //Rotate the player
        t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
        t.eulerAngles = new Vector3(0f, t.eulerAngles.y, 0f);
    }
    void LateUpdate()
    {
        //Face the enemy with the chest
        Vector3 dirToLook = mouseWorld - t.position - new Vector3(0f, 0.1f, 0f);
        Quaternion rotation = Quaternion.LookRotation(dirToLook);
        chest.rotation = Quaternion.Euler(rotation.eulerAngles.x, chest.eulerAngles.y, rotation.eulerAngles.z);
    }
}

