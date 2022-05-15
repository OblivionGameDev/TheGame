using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponAim : MonoBehaviour
{
    public Rig assaultRiffleAimLayer;
    public Rig pistolAimLayer;
    public Rig leftHandPistolAimLayer;

    private PistolReloadBoolHandler pistolReloadBoolHandlerScript;



    // TEMPORARY
    public AmmoWidget ammoWidgetScript;
    public Ammo ammoScript;




    [HideInInspector] public bool assaultRiffleAimed = false;
    [HideInInspector] public bool pistolAimed = false;
    [HideInInspector] public Animator cameraAnimator;
    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public PlayerWeaponSwitchScript playerWeaponSwitchScript;
    
    private PlayerLocomotionScript playerLocomotionScript;
    private RaycastAssaultRiffle raycastAssaultRiffleScript;
    private RaycastPistol raycastPistolScript;
    private float aimDuration = 0.15f;
    private bool pistolIsAimedIsDone;
    private bool coroutineStarted;

    // Start is called before the first frame update
    void Awake()
    {
        pistolReloadBoolHandlerScript = GetComponentInChildren<PistolReloadBoolHandler>();
        playerLocomotionScript = GetComponent<PlayerLocomotionScript>();
        cameraAnimator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
        raycastAssaultRiffleScript = GameObject.FindGameObjectWithTag("AssaultRiffleInHand").GetComponent<RaycastAssaultRiffle>();
        raycastPistolScript = GameObject.FindGameObjectWithTag("PistolInHand").GetComponent<RaycastPistol>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Aiming AssaultRiffle
        if (playerControls.Player.WeaponAim.triggered && playerWeaponSwitchScript.assaultRiffleEquiped && !assaultRiffleAimed)
        {
            assaultRiffleAimed = true;
            cameraAnimator.SetBool("isAiming", true);
            playerLocomotionScript.xAxis.m_MaxSpeed = 100;
            playerLocomotionScript.yAxis.m_MaxSpeed = 100;
        }
        else if (playerControls.Player.WeaponAim.triggered && assaultRiffleAimed)
        {
            assaultRiffleAimed = false;
            cameraAnimator.SetBool("isAiming", false);
            playerLocomotionScript.xAxis.m_MaxSpeed = 200;
            playerLocomotionScript.yAxis.m_MaxSpeed = 200;
        }
        if (assaultRiffleAimed)
        {
            assaultRiffleAimLayer.weight += Time.deltaTime / aimDuration;
        }
        else if (!assaultRiffleAimed)
        {
            assaultRiffleAimLayer.weight -= Time.deltaTime / aimDuration;
        }

        // Aiming Pistol
        if (playerControls.Player.WeaponAim.triggered && playerWeaponSwitchScript.pistolEquipped && !pistolAimed)
        {
            pistolAimed = true;
            cameraAnimator.SetBool("isAiming", true);
            playerLocomotionScript.xAxis.m_MaxSpeed = 100;
            playerLocomotionScript.yAxis.m_MaxSpeed = 100;
        }
        else if (playerControls.Player.WeaponAim.triggered && pistolAimed)
        {
            pistolAimed = false;
            cameraAnimator.SetBool("isAiming", false);
            playerLocomotionScript.xAxis.m_MaxSpeed = 200;
            playerLocomotionScript.yAxis.m_MaxSpeed = 200;
        } 
        if (pistolAimed && pistolAimLayer.weight < 1)
        {
            pistolAimLayer.weight += Time.deltaTime / aimDuration;
        }
        else if (!pistolAimed) 
        {
            pistolAimLayer.weight -= Time.deltaTime / aimDuration;
            leftHandPistolAimLayer.weight = 0;
        }

        if (pistolAimed && !pistolReloadBoolHandlerScript.pistolReloadLeftHandIkBool)
        {
            leftHandPistolAimLayer.weight = 1;
        }

        // Needs to be updated every frame!
        raycastAssaultRiffleScript.UpdateBullets(Time.deltaTime);
        raycastPistolScript.UpdateBullets(Time.deltaTime);

        if (raycastAssaultRiffleScript.isFiring)
        {
            raycastAssaultRiffleScript.UpdateFiring(Time.deltaTime);
        }
        if(raycastPistolScript.isFiring)
        {
            raycastPistolScript.UpdateFiring(Time.deltaTime);
        }

    }

    void OnEnable()
    {
        playerControls.Enable();
        
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
}
