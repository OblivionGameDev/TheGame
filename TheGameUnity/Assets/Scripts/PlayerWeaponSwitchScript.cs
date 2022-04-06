using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponSwitchScript : MonoBehaviour
{

    public Rig pistolInHandLayer;
    public Rig assaultRiffleInHandLayer;
    private PlayerControls playerControls;
    private GameObject pistolOnLeg;
    private GameObject pistolInHand;
    private GameObject assaultRiffleOnBack; 
    private GameObject assaultRiffleInHand;

    private bool pistolEquipped;
    private bool assaultRiffleEquiped;
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
        if (playerControls.Player.EquipAssaultRiffle.triggered && !assaultRiffleEquiped && !pistolEquipped)
        {
            assaultRiffleOnBack.SetActive(false);
            assaultRiffleInHand.transform.GetChild(0).gameObject.SetActive(true);
            assaultRiffleInHandLayer.weight = 1f;
            assaultRiffleEquiped = true;
        } 
        else if (playerControls.Player.EquipAssaultRiffle.triggered && assaultRiffleEquiped && !pistolEquipped)
        {
            assaultRiffleOnBack.SetActive(true);
            assaultRiffleInHand.transform.GetChild(0).gameObject.SetActive(false);
            assaultRiffleInHandLayer.weight = 0f;
            assaultRiffleEquiped = false;
        }

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
