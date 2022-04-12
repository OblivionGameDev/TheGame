using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponRecoil : MonoBehaviour
{
    public PlayerLocomotionScript playerLocomotionScript;
    public Cinemachine.CinemachineImpulseSource cameraShake;
    public Vector2[] recoilPattern;
    public float duration;

    private float verticalRecoil;
    private float horizontalRecoil;
    private float time;
    private int index;
    
    int NextIndex(int index)
    {
        return (index + 1) % recoilPattern.Length;
    }

    public void Reset()
    {
        index = 0;
    }
    public void GenerateRecoil()
    {   
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;
        index = NextIndex(index);
    }
    
    void Awake()
    {
        playerLocomotionScript = GetComponentInParent<PlayerLocomotionScript>();
    }
    
    void Update()
    {
        if (time > 0)
        {
            playerLocomotionScript.yAxis.Value -= ((verticalRecoil/10) * Time.deltaTime) / duration;
            playerLocomotionScript.xAxis.Value -= ((horizontalRecoil/10) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
