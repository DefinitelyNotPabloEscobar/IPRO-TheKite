using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DefeatAnimationCrash
{
    public KiteMovementScript k;
    public Transform kite;

    public DefeatAnimationCrash(KiteMovementScript K)
    {
        this.k = K;
        kite = k.kite;
    }

    public void Update()
    {

    }
}
