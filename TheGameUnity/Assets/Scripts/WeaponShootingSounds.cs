using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingSounds : MonoBehaviour
{

    public AudioSource gunSound;
    public AudioSource assaultRiffleSound;

    private PlayerWeaponAim playerWeaponAimScript;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    

    void Awake()
    {
        playerWeaponAimScript = GetComponent<PlayerWeaponAim>();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerWeaponAimScript.playerControls.Player.Shoot.triggered && playerWeaponAimScript.pistolAimed && playerWeaponAimScript.playerWeaponSwitchScript.pistolEquipped)
        {
            gunSound.Play();
        }
        if (playerWeaponAimScript.shootButtonPressed && playerWeaponAimScript.assaultRiffleAimed && playerWeaponAimScript.playerWeaponSwitchScript.assaultRiffleEquiped && !assaultRiffleSound.isPlaying)
        {
            assaultRiffleSound.Play();
        }
    }
}
