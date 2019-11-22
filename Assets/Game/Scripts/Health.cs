using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Health : MonoBehaviour
{
    public int baseHealth;
    public float deathTime = 5;
    public bool banker;
    public Collider[] colliders;

    protected int health;
    public bool IsDead { get; protected set; }

    Limb.LimbType limb;
    Animator anim;
    LookAtIK lookAtIk;

    protected virtual void Start()
    {
        health = baseHealth;
        anim = GetComponentInChildren<Animator>();

        lookAtIk = GetComponent<LookAtIK>();
    }

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        if(!IsDead && health <= 0)
        {
            IsDead = true;
            Died();
        }
    }

    public virtual void TookDamage(int damage, Limb.LimbType limbType)
    {
        health -= damage;

        if (!IsDead && health <= 0)
        {
            IsDead = true;
            limb = limbType;
            Died();
        }
    }

    public virtual void GainHealth(int healthGained)
    {
        if (health + healthGained <= baseHealth)
            health += healthGained;
        else
            health = baseHealth;
    }

    protected virtual void Died()
    {
        if (!banker)
            GameManager.Instance.EnemyKilled();
        else
            GameManager.Instance.BankerKilled();

        if (lookAtIk)
            lookAtIk.enabled = false;

        if (colliders.Length > 0)
            foreach (Collider col in colliders)
                col.enabled = false;

        anim.SetBool("IsDead", true);
        if (limb == Limb.LimbType.Head)
            anim.SetBool("Headshot", true);
        else if (limb == Limb.LimbType.Body)
            anim.SetBool("Headshot", false);

        Destroy(gameObject, deathTime);
    }
}
