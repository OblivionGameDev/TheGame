using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingSounds : MonoBehaviour
{

    public AudioSource gunSound;
    public AudioSource assaultRiffleSound;

    private PlayerWeaponAim playerWeaponAimScript;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    private PlayerWeaponShoot playerWeaponShoot;

    

    void Awake()
    {
        playerWeaponAimScript = GetComponent<PlayerWeaponAim>();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
        playerWeaponShoot = GetComponent<PlayerWeaponShoot>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerWeaponShoot.shootButtonPressed && playerWeaponAimScript.playerWeaponSwitchScript.assaultRiffleEquiped && !assaultRiffleSound.isPlaying)
        {
            assaultRiffleSound.Play();
        }
    }
}
