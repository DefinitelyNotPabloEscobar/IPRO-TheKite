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

                if (Time.time - phaseStartedTime < 1) break;

                if (Mathf.Abs(slopeCalculator.CalculateSlopeAngle()) > 35 || Mathf.Abs(breath.position.y) >= 2)
                {
                    Error += diff / 333 * errorAmp;
                    errorAmp += errorAmpIncrease;
                }
                else errorAmp = errorAmpConst;
                break;

            case 1:

                if (Time.time - phaseStartedTime < 1) break;

                if (Mathf.Abs(slopeCalculator.CalculateSlopeAngle()) > 35 || Mathf.Abs(breath.position.y) >= 2)
                {
                    Error += diff / 142 * errorAmp;
                    errorAmp += errorAmpIncrease;
                }
                else errorAmp = errorAmpConst;
                break;

            case 2:

                if (Time.time - phaseStartedTime < 1) break;

                if (Mathf.Abs(slopeCalculator.CalculateSlopeAngle()) > 35 || Mathf.Abs(breath.position.y) >= 2)
                {
                    Error += diff / 125 * errorAmp;
                    errorAmp += errorAmpIncrease;
                }
                else errorAmp = errorAmpConst;
                break;

            case 3:

                if (Time.time - phaseStartedTime < 1) break;

                if (Mathf.Abs(slopeCalculator.CalculateSlopeAngle()) > 35 || Mathf.Abs(breath.position.y) >= 2)
                {
                    Error += diff / 100 * errorAmp;
                    errorAmp += errorAmpIncrease;
                }
                else errorAmp = errorAmpConst;
                break;
        }

        if (Error > 1) Error = 1;
        CheckTimer();
    }
}
