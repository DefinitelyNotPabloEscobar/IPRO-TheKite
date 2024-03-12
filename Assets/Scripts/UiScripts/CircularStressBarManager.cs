using System.Collections;
using UnityEngine;

public class CircularStressBarManager: MonoBehaviour
{
    public RectTransform knob;
    public float stressValue;

    private float min = -25f;
    private float max = 125f;

    public float stressReal = 0f;
    public float seconds = 3f;
    private bool done = false;

    public void ChangeStressValue(float stressValue)
    {
        float amount = (stressValue / 100.0f) * 180f / 360f;
        float buttonAngle = amount * 360;
        knob.localEulerAngles = new Vector3(0, 0, -buttonAngle);
    }

    public void Start()
    {

    }

    public void StartAnimation()
    {
        if(done) return;
        StartCoroutine(MovingStress(seconds));
        done = true;
    }

    public void Update()
    {
        ChangeWithPercentage(stressReal);
    }

    public void ChangeWithPercentage(float stress)
    {
        if(stress > 1) stress = 1;
        if(stress < 0) stress = 0;
        ChangeStressValue(MapValue(stress));
    }

    float MapValue(float value)
    {
        return value * (max - min) + min;
    }

    IEnumerator MovingStress(float seconds)
    {
        var t = 0f;
        while(t < seconds)
        {
            t += Time.deltaTime;
            stressReal = Mathf.Lerp(0, stressValue, t / seconds);
            yield return null;
        }
    }


}