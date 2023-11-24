using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraHeight = 10f; // Desired height of the camera above the kite
    public KiteMovementScript kite;
    public float kiteScreanPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (kite != null)
        {
            float x = kite.getRadius() * Mathf.Cos((kite.getAngle() - kiteScreanPosition) * Mathf.Deg2Rad); 
            float y = kite.transform.position.y + cameraHeight;
            float z = kite.getRadius() * Mathf.Sin((kite.getAngle() - kiteScreanPosition) * Mathf.Deg2Rad);

            Vector3 kiteViewPosition = new Vector3(x, y, z);

            // Make the camera look at the kite's position
            transform.LookAt(kiteViewPosition);
        }
    }
}
