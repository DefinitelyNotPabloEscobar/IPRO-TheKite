using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform kite; // Reference to the kite transform
    public float cameraHeight = 10f; // Desired height of the camera above the kite

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (kite != null)
        {
            // Get the position of the kite
            Vector3 kitePosition = kite.position;

            // Make the camera look at the kite's position
            transform.LookAt(kitePosition);
        }
    }
}
