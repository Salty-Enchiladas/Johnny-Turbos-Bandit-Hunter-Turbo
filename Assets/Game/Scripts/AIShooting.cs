using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : Shooting
{
	protected override void Update() { }

    public override void Fire()
    {
        if(!Reloading)
        {
            ammo--;
            if (muzzleFlash)
                Instantiate(muzzleFlash, gunPoint.transform.position, gunPoint.transform.rotation);
            RaycastHit hit;

            if(Physics.Raycast(gunPoint.transform.position, gunPoint.transform.forward, out hit, Mathf.Infinity))
            {
                if (gunSource)
                    gunSource.Play();
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                switch (hit.transform.tag)
                {
                    case "Player":
                        if (bloodEffect)
                            Instantiate(bloodEffect, hit.point, Quaternion.identity);
                        hit.transform.GetComponent<Health>().TookDamage(damage);
                        break;
                    case "Enemy":
                        if (bloodEffect)
                            Instantiate(bloodEffect, hit.point, Quaternion.identity);
                        hit.transform.GetComponent<Limb>().TookDamage(damage);
                        break;
                    case "Cactus":
                        if (cactusBlood)
                            Instantiate(cactusBlood, hit.point, Quaternion.identity);
                        position = hit.point + (hit.normal * .1f);
                        rotation = Quaternion.LookRotation(hit.normal);
                        if (bulletHole != null)
                            Instantiate(bulletHole, position, rotation);
                        break;
                    case "Environment":
                        position = hit.point + (hit.normal * .1f);
                        rotation = Quaternion.LookRotation(hit.normal);
                        if (bulletHole != null)
                            Instantiate(bulletHole, position, rotation);
                        break;
                }
                
            }

            if (ammo <= 0)
            {
                Reloading = true;
                StartCoroutine(Reload());
            }
        }
    }
}
