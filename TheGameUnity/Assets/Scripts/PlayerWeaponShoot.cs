using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponShoot : MonoBehaviour
{
    private PlayerControls playerControls;
    [HideInInspector] public bool shootButtonPressed;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Shoot.performed += ctx => shootButtonPressed = true;
        playerControls.Player.Shoot.canceled += ctx => shootButtonPressed = false;
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
}
