using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class SwitchingCameras : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;
    public Animator cinemachineAnimator;
    public PlayableDirector firstCutScene;
    private PlayerControls playerControls;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    public void SwitchPriority()
    {
       firstCutScene.Play();
    }




    void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.CameraSwitch.performed += ctx => SwitchPriority();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
}
