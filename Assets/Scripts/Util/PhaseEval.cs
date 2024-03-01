using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEval
{
    private int difficulty;

    public PhaseEval(int d)
    {
        this.difficulty = d;
    }

    public float EvalInhale(InhaleManager im)
    {
        var inhaleError = im.ReturnErrorValue();
        var ie = inhaleError;

        switch (difficulty)
        {
            case 0:
            default:
                if (inhaleError <= 0.5) ie = 0f;
                ie = ie / 3;
                break;

            case 1:
                if (inhaleError <= 0.4) ie = 0f;
                ie = ie / 3;
                break;

            case 2:
                if (inhaleError <= 0.3) ie = 0f;
                ie = ie / 3;
                break;

            case 3:
                ie = ie / 3;
                break;

        }

        Debug.Log("|" + Time.time + "| Inhale Phase ended with " + ie);
        return ie;
    }

    public float EvalHold(HoldManager hm)
    {
        var holdError = hm.ReturnErrorValue();
        var hd = holdError;

        switch (difficulty)
        {
            case 0:
            default:
                if (holdError <= 0.5) hd = 0f;
                hd = hd / 3;
                break;

            case 1:
                if (holdError <= 0.4) hd = 0f;
                hd = hd / 3;
                break;

            case 2:
                if (holdError <= 0.2) hd = 0f;
                hd = hd / 3;
                break;

            case 3:
                hd = hd / 3;
                break;

        }

        Debug.Log("|" + Time.time + "| Hold Phase ended with " + hd);
        return hd;
    }


    public float EvalExhale(ExhaleManager em)
    {
        var exhaleError = em.ReturnErrorValue();
        var ee = exhaleError;

        switch (difficulty)
        {
            case 0:
            default:
                if (exhaleError <= 0.5) ee = 0f;
                ee = ee / 3;
                break;

            case 1:
                if (exhaleError <= 0.4) ee = 0f;
                ee = ee / 3;
                break;

            case 2:
                if (exhaleError <= 0.3) ee = 0f;
                ee = ee / 3;
                break;

            case 3:
                ee = ee / 3;
                break;

        }

        Debug.Log("|" + Time.time + "| Exhale Phase ended with " + ee);
        return ee;
    }

    public bool EvalPhase(float total)
    {
        switch (difficulty)
        {
            case 0:
            default:
                if(total <= 0.5)
                    return true;
                else return false;

            case 1:
                if (total <= 0.5)
                    return true;
                else return false;

            case 2:
                if (total <= 0.5)
                    return true;
                else return false;

            case 3:
                if (total <= 0.5)
                    return true;
                else return false;

        }
    }
}
