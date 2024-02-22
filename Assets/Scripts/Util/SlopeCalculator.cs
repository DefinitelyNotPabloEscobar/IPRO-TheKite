using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCalculator: MonoBehaviour
{

    public float lateValue = float.NegativeInfinity;
    public float newValue = float.NegativeInfinity;
    private float t = 0.5f;

    private Transform breath;

    public SlopeCalculator(float t, Transform breath) 
    {
        this.t = t;
        this.breath = breath;
    }


    public void ReceiveYValue(float yValue)
    {
        if(newValue == float.NegativeInfinity) 
        {
            newValue = yValue;
        }
        else
        { 
            lateValue = newValue;
            newValue = yValue;
        }
    }


    public float CalculateSlopeAngle()
    {
        float dy = newValue - lateValue;

        // Calculate the angle of the slope
        float slopeAngleRadians = Mathf.Atan(dy / t);
        float slopeAngleDegrees = slopeAngleRadians * (180 / Mathf.PI);
        return slopeAngleDegrees;
    }

    public IEnumerator Obtain()
    {
        while (true)
        {
            yield return new WaitForSeconds(t);
            ReceiveYValue(breath.position.y);
        }
    }
}
