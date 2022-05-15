using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoilTrigger : MonoBehaviour
{
    private Animator gunsAnimator;

    void Awake()
    {
        gunsAnimator = GetComponentInParent<Animator>();
    }
    void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "resetPistolRecoilTrigger":
            ResetTriggerPistol();
            break;
            case "resetAssaultRiffleTrigger":
            ResetTriggerAssaultRiffle();
            break;
        }
    }

    void ResetTriggerPistol()
    {
        gunsAnimator.ResetTrigger("pistolRecoil");
    }

    void ResetTriggerAssaultRiffle()
    {
        gunsAnimator.ResetTrigger("assaultRiffleRecoil");
    }
}
