using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliding : MonoBehaviour
{
    LookAtMouse lm;
    Movement mv;
    public AnimationManager am;
    CharacterController cc;
    public GameObject flash;
    BoxCollider slidingcollider;
    public LayerMask groundLayer;
    float slidingRight;
    float colliderHeight;
    Vector3 colliderCenter;
    bool retracing = false;
    public Slider slider;
    public float cooldown = 5f;
    private float cdTimer;
    public float pushStrength = 3f;
    public bool isSliding = false;
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
        cdTimer = 0f;
        lm = this.GetComponent<LookAtMouse>();
        mv = this.GetComponent<Movement>();
        cc = this.GetComponent<CharacterController>();
        slidingcollider = this.GetComponent<BoxCollider>();
        colliderHeight = cc.height;
        colliderCenter = cc.center;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && cdTimer <= 0f)
        {
            if (!mv.inAir && ((mv.velocity.x < -1f) || (mv.velocity.x > 1f)))
            {
                Slide();
                am.Slide();
                mv.isSliding = true;
                cc.detectCollisions = false;
                cc.center = new Vector3(0f, -0.9f, 0f);
                cc.height = 0f;
                slidingcollider.enabled = true;
                Invoke("GetUp", 1f);
            }
        }
        if (cdTimer > 0f)
        {
            CooldownTick();
        }
        if (retracing)
        {
            GetUp();
        }
        slider.value = Mathf.Max(cdTimer / cooldown, 0f);
    }
    void Slide()
    {
        cdTimer = cooldown;
        mv.maxRestrictSpeedScale = 0.01f;
        mv.recoverDuration = 0.75f;
        mv.AddVelocity(new Vector3(1, 0, 0) * pushStrength * (mv.velocity.x < -1f ? -1 : 1));
        slidingRight = lm.playerIsRight ? 1 : -1;
        mv.CeaseControl();
        isSliding = true;
    }
    void GetUp()
    {
        if (Physics.CheckSphere(t.position, 0.1f, groundLayer))
        {
            Debug.Log("retracing");
            retracing = true;
            Retrace();
        }
        else
        {
            retracing = false;
            cc.center = colliderCenter;
            cc.height = colliderHeight;
            slidingcollider.enabled = false;
            cc.detectCollisions = true;
            isSliding = false;
        }

    }
    void Retrace()
    {
        cc.Move(slidingRight * Vector3.right * Time.deltaTime * 5f);
        Debug.Log(t.position);
    }
    void CooldownTick()
    {
        cdTimer -= Time.deltaTime;
        if (cdTimer <= 0f)
        {
            flash.GetComponent<Flash>().FlashImage();
        }
    }
}
