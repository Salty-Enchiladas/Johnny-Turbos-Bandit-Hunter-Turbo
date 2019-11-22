using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(Vector3.zero);
    }
}
