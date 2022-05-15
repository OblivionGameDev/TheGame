using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAssaultRiffle : MonoBehaviour
{
     [HideInInspector] public PlayerControls playerControls;

    public Animator animator;
    public Transform leftHand;
    public GameObject assaultRiffleMagazine;
    public GameObject MagazineInHand;
    public GameObject assaultRiffleMagazinePrefab;
    public Transform assaultRiffleMagazineInstantiate;
    public AmmoWidget ammoWidget;
    private GameObject droppedMagazine;
    private RaycastAssaultRiffle raycastAssaultRiffleScript;
    private Ammo ammoScript;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    


    private WeaponMagazineAnimationEvents weaponMagazineAnimationEventsScript;

    GameObject magazineInAssaultRiifle;

    // Start is called before the first frame update
    void Awake()
    {
        ammoScript = GetComponent<Ammo>();
        weaponMagazineAnimationEventsScript = GetComponentInChildren<WeaponMagazineAnimationEvents>();
        playerControls = new PlayerControls();
        raycastAssaultRiffleScript = GetComponentInChildren<RaycastAssaultRiffle>();
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
           if (playerWeaponSwitchScript.assaultRiffleEquiped) animator.SetTrigger("assaultRiffleReload");
        }
        if (raycastAssaultRiffleScript.isFiring)
        {
            ammoWidget.Refresh(ammoScript.ammoCount);
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "detachAssaultRiffleMagazine":
            DetachMagazine();
            break;
            case "dropAssaultRiffleMagazine":
            DropMagazine();
            break;
            case "createAssaultRiffleMagazine":
            CreateNewMagazine();
            break;
            case "attachAssaultRiffleMagazine":
            AttachNewMagazine();
            break;
        }
    }

    void DetachMagazine()
    {
        assaultRiffleMagazine.SetActive(false);
        MagazineInHand.SetActive(true);
    }

    void DropMagazine()
    {
        MagazineInHand.SetActive(false);
        droppedMagazine = Instantiate(assaultRiffleMagazinePrefab, assaultRiffleMagazineInstantiate.transform.position, assaultRiffleMagazineInstantiate.transform.rotation);
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
        assaultRiffleMagazine.SetActive(true);
        ammoScript.assaultRiffleAmmoCount = ammoScript.assaultRiffleClipSize;
        ammoScript.ammoCount = ammoScript.assaultRiffleAmmoCount;
        ammoWidget.Refresh(ammoScript.ammoCount);
        animator.ResetTrigger("assaultRiffleReload");
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
