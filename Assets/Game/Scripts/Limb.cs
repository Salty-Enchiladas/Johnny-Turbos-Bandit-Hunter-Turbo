using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    Health health;

    public enum LimbType
    {
        Head,
        Body
    };

    public LimbType limb;

    private void Start()
    {
        health = transform.root.GetComponent<Health>();
    }

    public void TookDamage(int damage)
    {
        if(health)
            health.TookDamage(damage);
    }
}
