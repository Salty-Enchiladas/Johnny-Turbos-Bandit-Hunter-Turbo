using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public float forceAmount = 5f;
    public AudioSource gunSource;
    public GameObject bulletHole;
    Rigidbody target;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        gunSource.PlayOneShot(gunSource.clip);

        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            Vector3 position = hit.point + (hit.normal * .01f);
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            if(bulletHole != null)
            {
                GameObject temp = Instantiate(bulletHole, position, rotation);
                temp.transform.SetParent(hit.transform);
            }
        }
    }

    public void SetTarget(Rigidbody targetSign)
    {
        target = targetSign;
    }

    public void RemoveTarget()
    {
        target = null;
    }

    public void ShootSign(string methodName)
    {
        target.AddForce(mainCam.transform.forward * forceAmount, ForceMode.Impulse);
        Invoke(methodName, 1.5f);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}