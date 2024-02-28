using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldManager : InhaleManager
{
    public HoldManager(SlopeCalculator sc, int d, Transform b, Transform p, float pt) : base(sc, d, b, p, pt)
    {

    }

    public new void Calculate()
    {

        var diff = Mathf.Abs(Mathf.Abs(breath.position.y) - Mathf.Abs(predicted.position.y));
        switch (difficulty)
        {
            case 0:
            default:
                if (Mathf.Abs(slopeCalculator.CalculateSlopeAngle()) > 25)
                {
                    Error += diff / 1000 * errorAmp;
                    errorAmp += errorAmpIncrease;
                }
                else errorAmp = errorAmpConst;
                break;

            case 1:
                break;

            case 2:
                break;
        }

        if (Error > 1) Error = 1;
        Debug.Log("Hold error " + Error);
        CheckTimer();
    }
}
