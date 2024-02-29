using ScottPlot.Drawing.Colormaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObstaclePositionMain : MonoBehaviour
{
    public KiteMovementScript kite;
    public Transform centerPos;
    public float offsetY = 144f;
    public float offsetY2 = 175f;
    public float offsetX = -55f;
    public float offsetX2 = 0f;
    public float radiusOffSet = 0f;
    public float radiusOffSet2 = 5f;

    public GameObject objectGroup;
    public GameObject objectGroup2;

    private bool Done = false;

    private void Update()
    {
        if (Done) return;

        objectGroup.SetActive(false);
        objectGroup2.SetActive(false);

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
                    offsetX2 *= 0.5f;
                    break;
                case 3:
                    StartCoroutine(ChangerOfLife(cycleTime, kite.holdDuration + kite.exhaleDuration + kite.inhaleDuration * 2 + 2));
                    offsetX = -offsetX;
                    offsetX2 = -offsetX2;
                    break;
            }
            Done = true;
        }

    }


    private void FindExhalePosition(float seconds)
    {
        float randomNumber = Random.Range(0f, 1f);
        if(Mathf.RoundToInt(randomNumber) == 0)
        {
            objectGroup.SetActive(true);
            objectGroup2.SetActive(false);

            float updatedAngle = kite.angle + offsetX + kite.angularSpeed * seconds;

            float x = (kite.radius + radiusOffSet) * Mathf.Cos(updatedAngle * Mathf.Deg2Rad);
            float y = transform.position.y;
            float z = (kite.radius + radiusOffSet) * Mathf.Sin(updatedAngle * Mathf.Deg2Rad);
            Vector3 drawVector = new Vector3(x, y, z);

            transform.position = drawVector;
            transform.rotation = Quaternion.Euler(0f, -updatedAngle + offsetY, 0f);
        }
        else
        {
            objectGroup.SetActive(false);
            objectGroup2.SetActive(true);

            float updatedAngle2 = kite.angle + offsetX2 + kite.angularSpeed * seconds;

            float x2 = (kite.radius + radiusOffSet2) * Mathf.Cos(updatedAngle2 * Mathf.Deg2Rad);
            float y2 = objectGroup2.transform.position.y;
            float z2 = (kite.radius + radiusOffSet2) * Mathf.Sin(updatedAngle2 * Mathf.Deg2Rad);
            Vector3 drawVector2 = new Vector3(x2, y2, z2);

            objectGroup2.transform.rotation = Quaternion.Euler(0f, -updatedAngle2 + offsetY2, 0f);
            objectGroup2.transform.position = drawVector2;
        }
    }

    IEnumerator ChangerOfLife(float seconds, float fowardTime)
    {
        FindExhalePosition(fowardTime);
        yield return new WaitForSeconds(seconds);
        StartCoroutine(ChangerOfLife(seconds, fowardTime));

    }


}
