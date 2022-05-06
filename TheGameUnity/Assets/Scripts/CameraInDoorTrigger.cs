using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInDoorTrigger : MonoBehaviour
{
    private Animator cameraAnimator;    
    private bool playerIsInDoor;
    // Start is called before the first frame update
    void Awake()
    {
        cameraAnimator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !cameraAnimator.GetBool("isAiming"))
        {
            
            cameraAnimator.SetBool("isAiming", true);
        }
        else if(other.tag == "Player" && cameraAnimator.GetBool("isAiming"))
        {

            cameraAnimator.SetBool("isAiming", false);
        }
    }
    
}
