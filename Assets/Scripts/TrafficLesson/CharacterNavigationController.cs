using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CharacterNavigationController : MonoBehaviour
{
    private Vector3 destination;
    private NavMeshAgent agent;
    private Animator _animator;
    public float minDistance = 2f;
    [HideInInspector]
    public bool reachedDestination = false;

    public float motionSpeed = 1.2f;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(1.7f, 2.3f);
        _animator = GetComponent<Animator>();
        _animator.SetFloat("MotionSpeed", motionSpeed);
    }

    public void SetDestination(Vector3 destination)
    {
        reachedDestination = false;
        agent.SetDestination(destination);
    }

    private void Update()
    {
        _animator.SetFloat("Speed", agent.velocity.magnitude);
        if (agent.remainingDistance <= minDistance)
        {
            reachedDestination = true;
        }
    }
}
