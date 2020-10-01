using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    // Update is called once per frame
    Vector3 mousePosition;
    public Vector3 mouseWorld;
    Quaternion faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    public Camera cam;
    private float camdepth = 12.5f;
    public Transform hitbox;
    public Animator anim;
    Transform t;
    public Transform head;
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
        mouseWorld = cam.ScreenToWorldPoint(mousePosition);
        hitbox.position = mouseWorld;
        if (mouseWorld.x < t.position.x - 0.1f)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        else if (mouseWorld.x > t.position.x + 0.1f)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        Debug.Log(faceRotation.eulerAngles);
        t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
        t.eulerAngles = new Vector3(0f, t.eulerAngles.y, 0f);
    }
    void LateUpdate()
    {
        Vector3 dirToLook = mouseWorld - t.position;
        Quaternion rotation = Quaternion.LookRotation(dirToLook);
        head.rotation = rotation;
        chest.rotation = Quaternion.Euler(rotation.eulerAngles.x, chest.eulerAngles.y + 35f, rotation.eulerAngles.z);
    }
    private void OnAnimatorIK()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, weapon.position);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKPosition(AvatarIKGoal.RightHand, weapon.position);
    }
}

