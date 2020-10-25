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
    BoxCollider slidingcollider;
    float colliderHeight;
    Vector3 colliderCenter;
    public Slider slider;
    public float cooldown = 5f;
    private float cdTimer;
    public float pushStrength = 3f;
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && cdTimer <= 0f)
        {
            if (!mv.inAir && ((lm.playerIsRight && mv.velocity.x < -5f) || (!lm.playerIsRight && mv.velocity.x > 5f)))
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
        slider.value = Mathf.Max(cdTimer / cooldown, 0f);
    }
    void Slide()
    {
        cdTimer = cooldown;
        mv.maxRestrictSpeedScale = 0.01f;
        mv.recoverDuration = 1f;
        mv.AddVelocity(new Vector3(1, 0, 0) * pushStrength * (lm.playerIsRight ? -1 : 1));
        mv.CeaseControl();

    }
    void GetUp()
    {
        cc.center = colliderCenter;
        cc.height = colliderHeight;
        slidingcollider.enabled = false;
        cc.detectCollisions = true;
    }
    void CooldownTick()
    {
        cdTimer -= Time.deltaTime;
    }
}
