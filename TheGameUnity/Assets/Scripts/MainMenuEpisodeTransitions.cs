using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEpisodeTransitions : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void SetTriggerScene1to2()
    {
        animator.SetTrigger("Scene1-2");
    }
    public void SetTriggerScene2to1()
    {
        animator.SetTrigger("Scene2-1");
    }
}
