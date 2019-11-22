using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("Basic Info")]
    public int damage;
    public int clipSize;
    public float reloadTime;

    [Space, Header("Gun Effects")]
    public GameObject muzzleFlash;
    public float muzzleFlashLength;
    public GameObject gunPoint;
    public GunRecoil gunRecoil;
    public Animator reloadAnimator;

    [Space, Header("Hit Effects")]
    public Image hitMarkers;
    public Color hitMarkerHeadShotColor;
    public GameObject bulletHole;
    public GameObject bloodEffect;
    public GameObject cactusBlood;

    [Space, Header("UI")]
    public TextMeshProUGUI ammoCount;
    public GameObject reloadText;
    public AudioSource gunSource;
    public Image reticle;

    [Space, Header("Aiming")]
    public bool canAim;
    public GameObject aim;
    public Vector3 aimPosition;
    public float aimSpeed;
    public float aimFoV;
    public float aimFoVSpeed;

    protected Camera cam;

    protected int ammo;
    protected int muzzleIndex;
    public bool Reloading { get; protected set; }
    public bool Aiming { get; protected set; }

    protected float baseFoV;
    protected Vector3 baseAimPosition;

    private void Start()
    {
        ammo = clipSize;
        cam = Camera.main;

        if(canAim)
        {
            baseFoV = cam.fieldOfView;
            baseAimPosition = aim.transform.localPosition;
        }
    }

    protected virtual void Update()
    {
        if (Settings.Instance.MenuActive) return;
        RedDot();
        if (canAim)
            Aim();
        Shoot();

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reloading = true;
            StartCoroutine(Reload());
        }
    }

    void RedDot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag.Equals("Enemy"))
                reticle.color = Color.red;
            else
                reticle.color = Color.white;
        }
        else
            reticle.color = Color.white;
    }

    void Aim()
    {
        if(Input.GetKey(KeyCode.Mouse1))
        {
            Aiming = true;
            aim.transform.localPosition = Vector3.Lerp(aim.transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFoV, aimFoVSpeed * Time.deltaTime);
        }
        else
        {
            Aiming = false;
            aim.transform.localPosition = Vector3.Lerp(aim.transform.localPosition, baseAimPosition, aimSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFoV, aimFoVSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Reloading)
        {
            ammo--;
            if (ammoCount)
                UpdateAmmoCount();

            GameManager.Instance.ShotFired();
            Fire();
            gunRecoil.Fire();

            if (ammo <= 0)
            {
                Reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    public virtual void Fire()
    {
        if(muzzleFlash)
            Instantiate(muzzleFlash, gunPoint.transform.position, gunPoint.transform.rotation);
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
        {
            if (gunSource)
                gunSource.Play();
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            switch (hit.transform.tag)
            {
                case "Enemy":
                    if (bloodEffect)
                        Instantiate(bloodEffect, hit.point, Quaternion.identity);
                    if (hitMarkers)
                        StartCoroutine(HitMarkers());

                    GameManager.Instance.EnemyHit();
                    Limb limb = hit.transform.GetComponent<Limb>();

                    if (limb.limb == Limb.LimbType.Body)
                    {
                        hitMarkers.color = Color.red;
                        limb.TookDamage(damage);
                    }
                    else if (limb.limb == Limb.LimbType.Head)
                    {
                        GameManager.Instance.Headshot();
                        hitMarkers.color = hitMarkerHeadShotColor;
                        limb.TookDamage(damage * 2);
                    }
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
    }

    protected virtual IEnumerator Reload()
    {
        if(reloadText)
            reloadText.SetActive(true);

        if (reloadAnimator)
            reloadAnimator.SetBool("Reloading", true);

        for(float i = reloadTime; i > 0; i--)
        {
            if(i == 1 && reloadAnimator)
                reloadAnimator.SetBool("Reloading", false);
            yield return new WaitForSeconds(1);
        }
        ammo = clipSize;
        if(ammoCount)
            UpdateAmmoCount();
        if (reloadText)
            reloadText.SetActive(false);
        Reloading = false;
    }

    protected virtual IEnumerator HitMarkers()
    {
        hitMarkers.gameObject.SetActive(true);
        yield return new WaitForSeconds(.3f);
        hitMarkers.gameObject.SetActive(false);
    }

    protected virtual void UpdateAmmoCount()
    {
        if(ammoCount)
        ammoCount.text = ammo + " / " + clipSize;
    }
}
