using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10;
    public float aimSpeed = 5;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

    [Space, Header("Rotation")]
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 4F;
    public float sensitivityY = 4F;

    public float aimSensitivityX = 1f;
    public float aimSensitivityY = 1f;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    Shooting shooting;

    Vector3 movement;
    Camera cam;
    Rigidbody rb;

    private void Start()
    {
        cam = Camera.main;
        shooting = GetComponent<Shooting>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensitivityX = 1;
        sensitivityY = 1;
    }

    private void Update()
    {
        if (Settings.Instance.MenuActive) return;
        rb.velocity = Vector3.zero;
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = cam.transform.TransformDirection(movement);
        movement = Vector3.ClampMagnitude(movement, 1);

        if (!shooting.Aiming)
            movement *= speed * Time.deltaTime;
        else
            movement *= aimSpeed * Time.deltaTime;

        movement.y = 0f;
        transform.position += movement;

        if (!shooting.Aiming)
            Rotation(sensitivityX, sensitivityY);
        else
            Rotation(sensitivityX * .5f, sensitivityY * .5f);
    }

    void Rotation(float _sensitivityX, float _sensitivityY)
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * _sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * _sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * _sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}
