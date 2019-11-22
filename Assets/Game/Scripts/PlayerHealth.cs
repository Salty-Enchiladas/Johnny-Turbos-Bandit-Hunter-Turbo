using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [Space, Header("UI")]
    public Image healthBar;
    public GameObject hitEffect;
    public float hitEffectTime = .25f;

    protected override void Start()
    {
        base.Start();
        UpdateHealth();
    }

    public override void TookDamage(int damage)
    {
        base.TookDamage(damage);
        StartCoroutine(HitEffect());
        UpdateHealth();
    }

    IEnumerator HitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(hitEffectTime);
        hitEffect.SetActive(false);
    }

    public override void GainHealth(int healthGained)
    {
        base.GainHealth(healthGained);
        UpdateHealth();
    }

    void UpdateHealth()
    {
        healthBar.fillAmount = (float)health / baseHealth;
    }

    protected override void Died()
    {
        GameManager.Instance.PlayerDied();
        Destroy(gameObject);
    }
}
