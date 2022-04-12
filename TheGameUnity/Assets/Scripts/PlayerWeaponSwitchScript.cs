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
    private PlayerWeaponAim playerWeaponAim;

    private GameObject pistolOnLeg;
    private GameObject pistolInHand;
    private GameObject assaultRiffleOnBack; 
    private GameObject assaultRiffleInHand;

    [HideInInspector] public bool pistolEquipped;
    [HideInInspector] public bool assaultRiffleEquiped;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        playerWeaponAim = GetComponent<PlayerWeaponAim>();
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
            playerWeaponAim.assaultRiffleAimed = false;
            playerWeaponAim.cameraAnimator.SetBool("isAiming", false);
            assaultRiffleEquiped = false;
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
            playerWeaponAim.pistolAimed = false;
            playerWeaponAim.cameraAnimator.SetBool("isAiming", false);    
            pistolEquipped = false;
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
