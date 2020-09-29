using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private Transform cam;
    public float speed = 3f;
    void Start()
    {
        cam = this.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 moveposition = new Vector3(player.position.x, player.position.y, cam.position.z);
        cam.position = Vector3.Slerp(cam.position, moveposition, Time.deltaTime * speed);
        cam.position = new Vector3(cam.position.x, cam.position.y, moveposition.z);
    }
}
