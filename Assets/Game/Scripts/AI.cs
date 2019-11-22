using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float searchFrequency;
    public float searchDistance;
    public float minimumWalkDistance;
    public float maximumWalkDistance;

    public float minimumFireFrequency;
    public float maximumFireFrequency;

    bool firing;
    bool searchingForTarget;

    Quaternion direction;
    Animator anim;
    Transform player;
    LookAtIK lookAtIK;

    Transform aimTransform;
    NavMeshAgent agent;
    Vector3 point;
    Shooting shooting;
    Transform target;
    Health health;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        shooting = GetComponent<Shooting>();
        health = GetComponent<Health>();

        GameObject tempPlayer = GameObject.Find("Player");
        if (tempPlayer)
            player = tempPlayer.transform;
        GameObject tempAimTransform =GameObject.Find("AimTransform");
        if(tempAimTransform)
            aimTransform = tempAimTransform.transform;

        lookAtIK = GetComponent<LookAtIK>();
        lookAtIK.solver.target = aimTransform;

        if (RandomPoint(transform.position, Random.Range(minimumWalkDistance, maximumWalkDistance), out point))
            agent.SetDestination(point);
    }

    void Update()
    {
        if(health.IsDead)
        {
            agent.SetDestination(transform.position);
            return;
        }
        if(!searchingForTarget)
        {
            searchingForTarget = true;
            SearchForTarget();
            StartCoroutine(WaitToSearch());
        }

        if (target == null)
            return;

        anim.SetFloat("Horizontal", agent.velocity.x);
        anim.SetFloat("Vertical", agent.velocity.z);
        direction.y = 0;

        transform.LookAt(target.transform.position);

        if(Vector3.Distance(transform.position, point) < 1)
            if (RandomPoint(transform.position, Random.Range(minimumWalkDistance, maximumWalkDistance), out point))
                agent.SetDestination(point);

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, 4, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag.Equals("Player") || hit.transform.tag.Equals("Enemy"))
            {
                if (!firing && !shooting.Reloading)
                {
                    firing = true;
                    StartCoroutine(Fire());
                }
            }
            else
                agent.SetDestination(target.transform.position);
        }
        agent.SetDestination(target.transform.position);
    }

    void SearchForTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, searchDistance, enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            target = enemiesInRange[0].transform;
            lookAtIK.solver.target = target;
        }

        if (target == null && player != null)
        {
            target = player;
            lookAtIK.solver.target = aimTransform;
        }
    }

    IEnumerator WaitToSearch()
    {
        yield return new WaitForSeconds(searchFrequency);
        searchingForTarget = false;
    }

    IEnumerator Fire()
    {
        shooting.Fire();
        yield return new WaitForSeconds(Random.Range(minimumFireFrequency, maximumFireFrequency));
        firing = false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
