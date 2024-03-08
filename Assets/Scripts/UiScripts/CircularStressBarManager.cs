using UnityEngine;

public class CircularStressBarManager: MonoBehaviour
{
    public RectTransform knob;
    public float stressValue;

    private float min = -25f;
    private float max = 125f;


    public void ChangeStressValue(float stressValue)
    {
        float amount = (stressValue / 100.0f) * 180f / 360f;
        float buttonAngle = amount * 360;
        knob.localEulerAngles = new Vector3(0, 0, -buttonAngle);
    }

    public void Start()
    {
        ChangeWithPercentage(stressValue);
    }

    public void Update()
    {
        ChangeWithPercentage(stressValue);
    }

    public void ChangeWithPercentage(float stress)
    {
        if(stress > 1) stress = 1;
        if(stress < 0) stress = 0;
        stressValue = stress;
        ChangeStressValue(MapValue(stress));
    }

    float MapValue(float value)
    {
        return value * (max - min) + min;
    }


}