using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponAim : MonoBehaviour
{
    public Rig assaultRiffleAimLayer;
    public Rig pistolAimLayer;


    [HideInInspector] public bool assaultRiffleAimed = false;
    [HideInInspector] public bool pistolAimed = false;
    [HideInInspector] public Animator cameraAnimator;

    private PlayerControls playerControls;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    private RaycastAssaultRiffle raycastAssaultRiffleScript;
    private RaycastPistol raycastPistolScript;
    private float aimDuration = 0.15f;

    private bool shootButtonPressed;
    private bool shootButtonReleased;

    // Start is called before the first frame update
    void Awake()
    {
        cameraAnimator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
        raycastAssaultRiffleScript = GetComponentInChildren<RaycastAssaultRiffle>();
        raycastPistolScript = GetComponentInChildren<RaycastPistol>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Aiming AssaultRiffle
        if (playerControls.Player.WeaponAim.triggered && playerWeaponSwitchScript.assaultRiffleEquiped && !assaultRiffleAimed)
        {
            assaultRiffleAimed = true;
            cameraAnimator.SetBool("isAiming", true);
        }
        else if (playerControls.Player.WeaponAim.triggered && assaultRiffleAimed)
        {
            assaultRiffleAimed = false;
            cameraAnimator.SetBool("isAiming", false);
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
        }
        else if (playerControls.Player.WeaponAim.triggered && pistolAimed)
        {
            pistolAimed = false;
            cameraAnimator.SetBool("isAiming", false);
        } 
        if (pistolAimed)
        {
            pistolAimLayer.weight += Time.deltaTime / aimDuration;
        }
        else if (!pistolAimed) 
        {
            pistolAimLayer.weight -= Time.deltaTime / aimDuration;
        }

        // Shooting with Assault Riffle
        if (shootButtonPressed && assaultRiffleAimed && playerWeaponSwitchScript.assaultRiffleEquiped)
        {
            raycastAssaultRiffleScript.StartFiring();
        }
        if (shootButtonReleased && assaultRiffleAimed)
        {
            raycastAssaultRiffleScript.StopFiring();
            shootButtonPressed = false;
            shootButtonReleased = false;
        }
        // Shooting with Pistol
        if (playerControls.Player.Shoot.triggered && pistolAimed && playerWeaponSwitchScript.pistolEquipped)
        {
            raycastPistolScript.StartFiring();
        } 
        else 
        {
            raycastPistolScript.StopFiring();
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
        playerControls.Player.Shoot.performed += ctx => shootButtonPressed = true;
        playerControls.Player.Shoot.canceled += ctx => shootButtonReleased = true;
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
}
