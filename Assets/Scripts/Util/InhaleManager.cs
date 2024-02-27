using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhaleManager
{
    private float Error = 0f;
    private int difficulty = 0;
    private SlopeCalculator slopeCalculator;
    private Transform breath;
    private Transform predicted;

    public InhaleManager(SlopeCalculator sc, int d, Transform b, Transform p)
    {
        slopeCalculator = sc;
        difficulty = d;
        breath = b;
        predicted = p;
    }
    
    public void Calculate()
    {

    }

    public float ReturnErrorValue()
    {
        return Error;
    }

}
