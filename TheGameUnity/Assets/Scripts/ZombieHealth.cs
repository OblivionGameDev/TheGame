using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;

    private Animator zombieAnimator;
    private Collider zombieCollider;
    void Awake()
    {
        zombieCollider = GetComponentInChildren<BoxCollider>();
        zombieAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (health <= 0)
        {
            zombieAnimator.SetBool("isDead", true);
            zombieCollider.enabled = false;
        }
        Debug.Log(health);
    }
}
