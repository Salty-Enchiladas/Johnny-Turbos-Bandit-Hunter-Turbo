using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankerSpawner : MonoBehaviour
{
    public GameObject banker;
    public Transform[] spawnPoints;
    public float spawnFrequency;

    bool spawning;

    void Update()
    {
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        print("Spawn");
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(banker, point.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnFrequency);
        spawning = false;
    }
}
