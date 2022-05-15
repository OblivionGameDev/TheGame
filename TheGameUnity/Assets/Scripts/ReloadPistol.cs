using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ReloadPistol : MonoBehaviour
{
    [HideInInspector] public PlayerControls playerControls;

    public Animator animator;
    public Transform leftHand;
    public GameObject pistolMagazine;
    public GameObject MagazineInHand;
    public Transform pistolMagazineInstantiate;
    public AmmoWidget ammoWidget;
    private GameObject droppedMagazine;
    private RaycastPistol raycastPistolScript;
    private Ammo ammoScript;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    


    private WeaponMagazineAnimationEvents weaponMagazineAnimationEventsScript;

    GameObject magazineInPistol;

    // Start is called before the first frame update
    void Awake()
    {
        ammoScript = GetComponent<Ammo>();
        weaponMagazineAnimationEventsScript = GetComponentInChildren<WeaponMagazineAnimationEvents>();
        playerControls = new PlayerControls();
        raycastPistolScript = GetComponentInChildren<RaycastPistol>();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
    }

    void Start()
    {
        weaponMagazineAnimationEventsScript.WeaponAnimationEvent.AddListener(OnAnimationEvent);
        ammoWidget.Refresh(ammoScript.ammoCount);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerControls.Player.Reload.triggered || ammoScript.ammoCount <= 0)
        {
            if (playerWeaponSwitchScript.pistolEquipped) animator.SetTrigger("pistolReload");
        }
        if (raycastPistolScript.isFiring)
        {
            ammoWidget.Refresh(ammoScript.ammoCount);
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "detachMagazine":
            DetachMagazine();
            break;
            case "createNewMagazine":
            CreateNewMagazine();
            break;
            case "attachNewMagazine":
            AttachNewMagazine();
            break;
        }
    }

    void DetachMagazine()
    {
        droppedMagazine = Instantiate(pistolMagazine, pistolMagazineInstantiate.transform.position, pistolMagazineInstantiate.transform.rotation);
        droppedMagazine.AddComponent<BoxCollider>();
        droppedMagazine.AddComponent<Rigidbody>();
        
    }

    void CreateNewMagazine()
    {
        MagazineInHand.SetActive(true);
    }

    void AttachNewMagazine()
    {
        MagazineInHand.SetActive(false);
        ammoScript.pistolAmmoCount = ammoScript.pistolClipSize;
        ammoScript.ammoCount = ammoScript.pistolAmmoCount;
        ammoWidget.Refresh(ammoScript.ammoCount);
        animator.ResetTrigger("pistolReload");
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
