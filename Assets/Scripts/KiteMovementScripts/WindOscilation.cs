using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindOscilation : MonoBehaviour
{
    public Vector3 maxRotationAngles = new Vector3(10f, 10f, 10f); // Maximum rotation angles in local coordinates
    public float oscillationSpeed = 1f; // Speed of oscillation
    public float rotationSmoothness = 5f; // Smoothness of rotation

    private Quaternion initialRotation;
    private Vector3 targetLocalRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate target rotation within the specified range
        float offsetX = Mathf.Sin(Time.time * oscillationSpeed) * maxRotationAngles.x;
        float offsetY = Mathf.Sin(Time.time * oscillationSpeed * 0.75f) * maxRotationAngles.y; // Adjust oscillation speed for each axis
        float offsetZ = Mathf.Sin(Time.time * oscillationSpeed * 1.25f) * maxRotationAngles.z; // Adjust oscillation speed for each axis

        targetLocalRotation = initialRotation.eulerAngles + new Vector3(offsetX, offsetY, offsetZ);

        // Smoothly rotate towards the target rotation
        Quaternion targetQuaternion = Quaternion.Euler(targetLocalRotation);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetQuaternion, Time.deltaTime * rotationSmoothness);
    }
}
