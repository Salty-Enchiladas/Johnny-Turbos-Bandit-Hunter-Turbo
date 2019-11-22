using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBob : MonoBehaviour
{
    public Vector3 offset;
    public float headbobSpeed;
    public float headbobAmountX;
    public float headbobAmountY;
    [HideInInspector]
    public float headbobStepCounter;
    Vector3 parentLastPosition;

    float aimHeadbobSpeed;
    float aimHeadbobAmountX;
    float aimHeadbobAmountY;

    Shooting shooting;

    void Start()
    {
        shooting = transform.root.GetComponent<Shooting>();
        offset = transform.localPosition;
        parentLastPosition = transform.root.position;

        aimHeadbobSpeed = headbobSpeed * .65f;
        aimHeadbobAmountX = headbobAmountX * .65f;
        aimHeadbobAmountY = headbobAmountY * .65f;
    }

    void Update()
    {
       if(!shooting.Aiming)
        {
            headbobStepCounter += Vector3.Distance(parentLastPosition, transform.root.position) * headbobSpeed;
            transform.localPosition = offset + new Vector3(Mathf.Sin(headbobStepCounter) * headbobAmountX,
                (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1), 0);
        }
       else
        {
            headbobStepCounter += Vector3.Distance(parentLastPosition, transform.root.position) * aimHeadbobSpeed;
            transform.localPosition = offset + new Vector3(Mathf.Sin(headbobStepCounter) * aimHeadbobAmountX,
                (Mathf.Cos(headbobStepCounter * 2) * aimHeadbobAmountY * -1), 0);
        }
        parentLastPosition = transform.root.position;
    }
}
