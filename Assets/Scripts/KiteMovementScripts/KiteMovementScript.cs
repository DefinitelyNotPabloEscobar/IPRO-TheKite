using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteMovementScript : MonoBehaviour
{
    // Angle increment per frame
    public float angularSpeed = -10f; // Adjust the speed as needed
    public float angularSecSpeed = -2f;
    public float angularElevSpeed = 2f;

    public float elevationAmp = 10f;

    public int numberOfIndicatores = 10;

    public float elevationHeightAmp = 10f;
    public Transform kite;

    public float xRotation;

    public float yRotation;
    public float zRotation;

    public GameObject indicator;

    public float indicatorSpread = 50f;
    
    private float angle = 0f;
    private float secAngle = 0f;

    private float elevationAngle = 0f;
    private float secMax = 10f;
    private float radius;

    private List<GameObject> objectList = new List<GameObject>();
    void Start()
    {
        if(kite != null){
            radius = kite.position.z;
        }
        else {
            radius = 10f;
        }

        for (int i = 0; i < numberOfIndicatores; i++)
        {
            float x = radius * Mathf.Cos((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, (((Mathf.Sin((elevationAngle + (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad) + 1) / 2) * elevationHeightAmp) + 3, z);

            GameObject indicatorObj = Instantiate(indicator, drawVector, Quaternion.Euler(0f,0f,0f));
            objectList.Add(indicatorObj);
        }
    }

    void Update()
    {
        //if(angle > 360f || angle < -360f) angle = 0f;
        // Increment the angle based on the angular speed
        angle += angularSpeed * Time.deltaTime;
        secAngle += angularSecSpeed * Time.deltaTime;
        elevationAngle += angularElevSpeed * Time.deltaTime;

        if(secAngle > secMax || secAngle < -secMax) angularSecSpeed = -angularSecSpeed;

        // Calculate the new position using polar coordinates
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Update the object's position
        kite.position = new Vector3(x, (((Mathf.Sin(elevationAngle * Mathf.Deg2Rad)+1)/2)*elevationHeightAmp) + 3, z);

        Quaternion newRotation = Quaternion.Euler(
            Mathf.Sin((-elevationAngle - 45f) * Mathf.Deg2Rad)*elevationAmp, -angle - 180f, secAngle);
        kite.rotation = newRotation;

        MoveIndicatores();
    }

    private void MoveIndicatores(){
        for (int i = 0; i < numberOfIndicatores; i++)
        {
            float x = radius * Mathf.Cos((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin((angle - (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, (((Mathf.Sin((elevationAngle + (((float)i / (float)numberOfIndicatores)*indicatorSpread)) * Mathf.Deg2Rad) + 1) / 2) * elevationHeightAmp) + 3, z);

            objectList[i].transform.position = drawVector;
        }
    }
}
