using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsRecoilTrigger : MonoBehaviour
{
    private Animator gunsAnimator;

    void Awake()
    {
        gunsAnimator = GetComponentInParent<Animator>();
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
