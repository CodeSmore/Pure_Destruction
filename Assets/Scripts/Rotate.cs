using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * rotationSpeed));
    }
}
