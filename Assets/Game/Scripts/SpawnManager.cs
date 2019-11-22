using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnFrequency;
    public float minimumSpawnRange;
    public float maximumSpawnRange;

    bool spawning;
    GameObject player;

    float minimumSpawnFrequency = .1f;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update ()
    {
		if(!spawning)
        {
            spawning = true;
            StartCoroutine(Spawn());
        }
	}

    public void EnemyKilled()
    {
        if(spawnFrequency - .1f >= minimumSpawnFrequency)
            spawnFrequency -= .05f;
    }

    IEnumerator Spawn()
    {
        if (player == null) yield break;
        Vector3 point;
        float spawnRange = Random.Range(minimumSpawnRange, maximumSpawnRange);
        if (RandomPoint(transform.position, spawnRange, out point))
        {
            point.y = 0;
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            Instantiate(enemy, point, Quaternion.identity);
        }


        yield return new WaitForSeconds(spawnFrequency);
        spawning = false;
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
