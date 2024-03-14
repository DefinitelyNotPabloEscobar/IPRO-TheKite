using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class Rope : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public int segments = 10;
    public float maxCurveOffset = 0.5f;
    public float maxCurveOffsetY = 0.5f;
    public float ropeWidth = 0.1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
            return;
        }

        if (startTransform == null || endTransform == null)
        {
            Debug.LogError("Start or end transform not assigned!");
            return;
        }

        DrawCurvedRope();
    }

    void DrawCurvedRope()
    {
        lineRenderer.positionCount = segments + 1;
        Vector3[] ropePositions = new Vector3[segments + 1];

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float curveOffset = Mathf.Sin(t * Mathf.PI) * maxCurveOffset;
            float curveOffsetY = Mathf.Sin(t * Mathf.PI) * maxCurveOffsetY;

            Vector3 pointOnLine = Vector3.Lerp(startTransform.position, endTransform.position, t);
            ropePositions[i] = new Vector3(pointOnLine.x, pointOnLine.y + curveOffsetY, pointOnLine.z + curveOffset);
        }

        lineRenderer.SetPositions(ropePositions);
    }

    void LateUpdate()
    {
        DrawCurvedRope();
    }
}

