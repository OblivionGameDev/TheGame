using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAi : MonoBehaviour
{
    public enum AIState { idle, chasing};
    public AIState aiState = AIState.idle;   

    private Animator dogAnimator;
    private NavMeshAgent nm;
    private Transform dogTarget;
    // Start is called before the first frame update
    void Awake()
    {
        dogAnimator = GetComponent<Animator>();
        nm = GetComponent<NavMeshAgent>();
        dogTarget = GameObject.FindGameObjectWithTag("DogTarget").transform;
        StartCoroutine(DogLogic());
    }

    // Update is called once per frame
    void Update()
    {
        dogAnimator.SetFloat("Speed", nm.velocity.magnitude);
    }

    IEnumerator DogLogic()
    {
        while(true)
        {
            float dist = Vector3.Distance(dogTarget.position, transform.position);
            switch (aiState)
            {
                case AIState.idle:
                    if (dist > 5f)
                    {
                        aiState = AIState.chasing;
                    }
                    nm.SetDestination(transform.position);
                break;
                case AIState.chasing:
                    if (dist <= 0.5f)
                    {
                        aiState = AIState.idle;
                    }
                    nm.SetDestination(dogTarget.position);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
