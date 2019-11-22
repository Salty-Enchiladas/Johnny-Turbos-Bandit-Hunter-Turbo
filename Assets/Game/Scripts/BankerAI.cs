using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BankerAI : MonoBehaviour
{
    public float targetRange;

    Vector3 point;
    NavMeshAgent agent;
    Health health;

    bool destroyed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        point = GameObject.Find("BankLocation").transform.position;
        agent.SetDestination(point);
    }

    private void Update()
    {
        if(health.IsDead)
        {
            agent.SetDestination(transform.position);
            return;
        }
        if (!destroyed && agent.destination != Vector3.zero && Vector3.Distance(transform.position, point) < 1)
        {
            destroyed = true;
            GameManager.Instance.BankerSaved();
            Destroy(gameObject);
        }
    }
}
