using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxMovement : MonoBehaviour
{

    public float skyBoxMovement = 0.005f;
    public float radius = 1000f; 
    public Transform center;
    public Transform lightToMove;

    private float angle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (lightToMove != null && center != null)
        {
            Vector3 direction = lightToMove.position - center.position;
            angle = Mathf.Atan2(direction.z, direction.x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxMovement);
        if(lightToMove != null && center != null){
            float currentSkyboxRotation = Time.time * skyBoxMovement;
            angle = currentSkyboxRotation;
            float x = center.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = center.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            lightToMove.position = new Vector3(x, lightToMove.position.y, z);

            // Make the light always point toward the center
            Vector3 lookAtPosition = center.position;
            lookAtPosition.y = lightToMove.position.y;
            lightToMove.LookAt(lookAtPosition);
        }
    }
}
