using Assets.Scripts.Util;
using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEval
{
    private ExhaleManager exhaleManager;
    private InhaleManager inhaleManager;
    private HoldManager holdManager;

    public PhaseEval(ExhaleManager exhaleManager, InhaleManager inhaleManager, HoldManager holdManager)
    {
        this.exhaleManager = exhaleManager;
        this.inhaleManager = inhaleManager;
        this.holdManager = holdManager;
    }

    public float Eval()
    {
        return 0f;
    }
}
