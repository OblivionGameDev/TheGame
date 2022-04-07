using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponAim : MonoBehaviour
{
    public Rig assaultRiffleAimLayer;
    public Rig pistolAimLayer;

    [HideInInspector] public bool assaultRiffleAimed;
    [HideInInspector] public bool pistolAimed;

    private PlayerControls playerControls;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;

    private float aimDuration = 0.15f;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Aiming AssaultRiffle
        if (playerControls.Player.WeaponAim.triggered && playerWeaponSwitchScript.assaultRiffleEquiped && !assaultRiffleAimed)
        {
            assaultRiffleAimed = true;
        }
        else if (playerControls.Player.WeaponAim.triggered && assaultRiffleAimed)
        {
            assaultRiffleAimed = false;
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
        }
        else if (playerControls.Player.WeaponAim.triggered && pistolAimed)
        {
            pistolAimed = false;
        } 
        if (pistolAimed)
        {
            pistolAimLayer.weight += Time.deltaTime / aimDuration;
        }
        else if (!pistolAimed) 
        {
            pistolAimLayer.weight -= Time.deltaTime / aimDuration;
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
