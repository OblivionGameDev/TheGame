using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponSwitchScript : MonoBehaviour
{

    public Rig pistolInHandLayer;
    public Rig assaultRiffleInHandLayer;
    public Rig pistolWeaponPoseLayer;
    public Rig pistolAimLayer;
    public Rig assaultRiffleWeaponPoseLayer;
    public Rig assaultRiffleAimLayer;

    private PlayerControls playerControls;

    private GameObject pistolOnLeg;
    private GameObject pistolInHand;
    private GameObject assaultRiffleOnBack; 
    private GameObject assaultRiffleInHand;

    private bool pistolEquipped;
    private bool pistolAimed;
    private bool assaultRiffleEquiped;
    private bool assaultRiffleAimed;

    private float aimDuration = 0.15f;
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        pistolOnLeg = GameObject.FindGameObjectWithTag("PistolOnLeg");
        pistolInHand = GameObject.FindGameObjectWithTag("PistolInHand");
        assaultRiffleOnBack = GameObject.FindGameObjectWithTag("AssaultRiffleOnBack");
        assaultRiffleInHand = GameObject.FindGameObjectWithTag("AssaultRiffleInHand");

    }

    // Update is called once per frame
    void Update()
    {
        //Equipping AssaultRiffle
        if (playerControls.Player.EquipAssaultRiffle.triggered && !assaultRiffleEquiped && !pistolEquipped)
        {
            assaultRiffleOnBack.SetActive(false);
            assaultRiffleInHand.transform.GetChild(0).gameObject.SetActive(true);
            assaultRiffleInHandLayer.weight = 1f;
            assaultRiffleWeaponPoseLayer.weight =1f;
            assaultRiffleEquiped = true;
        } 
        else if (playerControls.Player.EquipAssaultRiffle.triggered && assaultRiffleEquiped && !pistolEquipped)
        {
            assaultRiffleOnBack.SetActive(true);
            assaultRiffleInHand.transform.GetChild(0).gameObject.SetActive(false);
            assaultRiffleInHandLayer.weight = 0f;
            assaultRiffleWeaponPoseLayer.weight = 0f;
            assaultRiffleAimLayer.weight = 0f;
            assaultRiffleEquiped = false;
        }
        // Aiming AssaultRiffle
        if (playerControls.Player.WeaponAim.triggered && assaultRiffleEquiped && !assaultRiffleAimed)
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

        // Equipping Pistol
        if (playerControls.Player.EquipPistol.triggered && !pistolEquipped && !assaultRiffleEquiped)
        {
            pistolInHand.transform.GetChild(0).gameObject.SetActive(true);
            pistolOnLeg.SetActive(false);
            pistolInHandLayer.weight = 1f;
            pistolEquipped = true;
        } 
        else if (playerControls.Player.EquipPistol.triggered && pistolEquipped && !assaultRiffleEquiped)
        {
            pistolInHand.transform.GetChild(0).gameObject.SetActive(false);
            pistolOnLeg.SetActive(true);
            pistolInHandLayer.weight = 0f;
            pistolAimLayer.weight = 0f;
            pistolEquipped = false;
        }

        // Aiming Pistol
        if (playerControls.Player.WeaponAim.triggered && pistolEquipped && !pistolAimed)
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
