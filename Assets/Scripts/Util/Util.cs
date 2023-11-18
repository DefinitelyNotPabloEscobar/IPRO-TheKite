using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Util
    {
        public static bool IsWithinThreshold(float value, float target, float threshold)
        {
            float difference = Mathf.Abs(value - target);
            return difference <= threshold;
        }
    }
}
