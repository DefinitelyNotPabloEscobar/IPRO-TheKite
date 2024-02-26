using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HousePositionMain : MonoBehaviour
{
    public KiteMovementScript kite;
    public Transform centerPos;
    public float offsetY = 144f;
    public float offsetX = -55f;
    public float radiusOffSet = 5f;

    public GameObject objectGroup;

    private bool Done = false;

    private void Update()
    {
        if (Done) return;

        objectGroup.SetActive(false);
        float cycleTime = kite.inhaleDuration + kite.holdDuration + kite.exhaleDuration;

        if (kite.CycleStartTime > 0)
        {
            switch (kite.difficulty)
            {
                case 0:
                    StartCoroutine(ChangerOfLife(cycleTime * 2, kite.exhaleDuration + kite.inhaleDuration));
                    break;
                case 1:
                    StartCoroutine(ChangerOfLife(cycleTime, kite.holdDuration + kite.exhaleDuration + kite.inhaleDuration*2));
                    break;
                case 2:
                    StartCoroutine(ChangerOfLife(cycleTime, kite.holdDuration + kite.exhaleDuration + kite.inhaleDuration * 2));
                    offsetX *= 0.5f;
                    break;
            }
            Done = true;
            objectGroup.SetActive(true);
        }

    }


    private void FindExhalePosition(float seconds)
    {
        float updatedAngle = kite.angle + offsetX + kite.angularSpeed * seconds;

        float x = (kite.radius + radiusOffSet) * Mathf.Cos(updatedAngle * Mathf.Deg2Rad);
        float y = transform.position.y;
        float z = (kite.radius + radiusOffSet) * Mathf.Sin(updatedAngle * Mathf.Deg2Rad);
        Vector3 drawVector = new Vector3(x, y, z);

        transform.position = drawVector;
        transform.rotation = Quaternion.Euler(0f, -updatedAngle + offsetY, 0f);
    }

    IEnumerator ChangerOfLife(float seconds, float fowardTime)
    {
        FindExhalePosition(fowardTime);
        yield return new WaitForSeconds(seconds);
        StartCoroutine(ChangerOfLife(seconds, fowardTime));

    }


}
