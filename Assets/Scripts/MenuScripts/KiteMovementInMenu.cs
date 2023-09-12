using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteMovementInMenu : MonoBehaviour
{

    public Transform kite;

    public float angularSecSpeed = -2f;

    public float angularSpeed = 0.2f;

    public float rX = 8.049f;
    public float rY = 106.73f;
    public float rZ = -93.0f;

    private float angle = 0f;
    private float secAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += angularSpeed * Time.deltaTime;
        secAngle += angularSecSpeed * Time.deltaTime;

        if(secAngle < -10f || secAngle > 0f){
            angularSecSpeed = -angularSecSpeed;
        }
        if(angle > 10f || angle < -10f){
            angularSpeed = -angularSpeed;
        }

        Quaternion newRotation = Quaternion.Euler(rX + angle, rY, rZ + secAngle);
        kite.rotation = newRotation;
    }
}
