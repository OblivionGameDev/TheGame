using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{
    public float chasingTreshold = 10f;
    public enum AIState { idle, chasing, attack, dead};
    public AIState aiState = AIState.idle;     

    private AiSensor aiSensorScript;
    private NavMeshAgent nm;
    private Transform target;
    private Animator zombieAnimator;
    private float dist;
    // Start is called before the first frame update
    void Awake()
    {
        aiSensorScript = GetComponent<AiSensor>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        zombieAnimator = GetComponent<Animator>();
        nm = GetComponent<NavMeshAgent>();
        StartCoroutine(ZombieLogic());
    }
    void Update()
    {
        zombieAnimator.SetFloat("Speed", nm.velocity.magnitude);
    }
    public void CheckAttackRange()
    {
        if (dist > 2f)
        {
            zombieAnimator.SetBool("isAttacking", false);
            aiState = AIState.chasing;
            
        }
    }
    IEnumerator ZombieLogic()
    {
        while(true)
        {
            dist = Vector3.Distance(target.position, transform.position);
            switch (aiState)
            {
                case AIState.idle:
                    if (aiSensorScript.isInSight) aiState = AIState.chasing;
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
                    if(aiSensorScript.isInSight)
                    {
                        transform.LookAt(target);
                    }
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
            yield return new WaitForSeconds(0.2f);
        }
    }
}
