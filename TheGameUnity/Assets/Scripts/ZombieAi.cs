using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{
    public float chasingTreshold = 10f;
    public enum AIState { idle, chasing, attack, dead};
    public AIState aiState = AIState.idle;     

    private NavMeshAgent nm;
    private Transform target;
    private Animator zombieAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        zombieAnimator = GetComponent<Animator>();
        nm = GetComponent<NavMeshAgent>();
        StartCoroutine(ZombieLogic());
    }
    void Update()
    {
        zombieAnimator.SetFloat("Speed", nm.velocity.magnitude);
    }
    IEnumerator ZombieLogic()
    {
        while(true)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            switch (aiState)
            {
                case AIState.idle:
                    if (dist < chasingTreshold)
                    {
                        aiState = AIState.chasing;
                    }
                    // Check if dead
                    if(zombieAnimator.GetBool("isDead"))
                    {
                        aiState = AIState.dead;
                    }
                nm.SetDestination(transform.position);
                break;
                case AIState.chasing:
                    zombieAnimator.SetBool("isRunning", true);
                    nm.SetDestination(target.position);
                    if (dist > chasingTreshold)
                    {
                        zombieAnimator.SetBool("isRunning", false);
                        aiState = AIState.idle;
                    }
                    if (dist <= 2f)
                    {
                        zombieAnimator.SetBool("isRunning", false);
                        aiState = AIState.attack;
                    }
                    // Check if dead
                    if(zombieAnimator.GetBool("isDead"))
                    {
                        aiState = AIState.dead;
                    }
                break;
                case AIState.attack:
                    zombieAnimator.SetBool("isAttacking", true);
                    transform.LookAt(target);
                    if (dist > chasingTreshold)
                    {   
                        zombieAnimator.SetBool("isAttacking", false);
                        aiState = AIState.idle;
                        Debug.Log("idle");
                    }
                    if (dist > 2f)
                    {
                        zombieAnimator.SetBool("isAttacking", false);
                        aiState = AIState.chasing;
                    }
                    // Check if dead
                    if(zombieAnimator.GetBool("isDead"))
                    {
                        aiState = AIState.dead;
                    }
                break;
                case AIState.dead:
                    nm.SetDestination(transform.position);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
