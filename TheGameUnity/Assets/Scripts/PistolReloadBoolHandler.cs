using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolReloadBoolHandler : MonoBehaviour
{
   public bool pistolReloadLeftHandIkBool;
    
    public void turnTheBoolOn()
    {
        pistolReloadLeftHandIkBool = true;
    }
    public void turnTheBoolOff()
    {
        pistolReloadLeftHandIkBool = false;
    }
}
