using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementPractice : MonoBehaviour
{
    public float cameraHeight = 10f; // Desired height of the camera above the kite
    public KiteMovementPractice kite;
    public float kiteScreanPosition;

    private bool dontLookAtKite = false;
    private float yStopped;
    private float angleReduce;

    public float angleReduceSpeed;


    void LateUpdate()
    {
        if(kite == null) return;

        if (!dontLookAtKite)
        {
            float x = kite.getRadius() * Mathf.Cos((kite.getAngle() - kiteScreanPosition) * Mathf.Deg2Rad); 
            float y = kite.transform.position.y + cameraHeight;
            float z = kite.getRadius() * Mathf.Sin((kite.getAngle() - kiteScreanPosition) * Mathf.Deg2Rad);

            Vector3 kiteViewPosition = new Vector3(x, y, z);

            // Make the camera look at the kite's position
            transform.LookAt(kiteViewPosition);
        }
        else
        {
            float x = kite.getRadius() * Mathf.Cos((kite.getAngle() - kiteScreanPosition + angleReduce) * Mathf.Deg2Rad);
            float z = kite.getRadius() * Mathf.Sin((kite.getAngle() - kiteScreanPosition + angleReduce) * Mathf.Deg2Rad);
            angleReduce += angleReduceSpeed * Time.deltaTime;

            Vector3 kiteViewPosition = new Vector3(x, yStopped, z);
             transform.LookAt(kiteViewPosition);
        }
    }

    public void DontLookAtKite()
    {
        dontLookAtKite=true;
        yStopped = kite.transform.position.y;
    }
}
