using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAI : MonoBehaviour
{
    public Transform target; // 추격 대상
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position); // 매 프레임마다 목표지점을 추격 대상으로 설정
    }
}

