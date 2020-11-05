using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscene : MonoBehaviour
{
    public SceneController sc;
    public GameObject cutsceneCamera;
    public GameObject mainCamera;
    public BossAttacks bossattack;
    public BossMovement bossmovement;
    public Animator bossanim;
    public LookAtMouse lam;
    public Movement mv;
    public Shooting s;
    public AbilityManager am;
    public GameObject hud;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StandUp", 4f);
        Invoke("ChangeCamera", 10f);
        Invoke("EnableBossMovement", 10f);
        Invoke("EnablePlayerControl", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StandUp()
    {
        bossanim.enabled = true;
    }
    void ChangeCamera()
    {
        cutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);
        
    }
    void EnableBossMovement()
    {
        bossattack.enabled = true;
        bossmovement.enabled = true;
    }
    void EnablePlayerControl()
    {
        sc.enabled = true;
        hud.SetActive(true);
        lam.enabled = true;
        mv.enabled = true;
        s.enabled = true;
        am.enabled = true;
    }
}
