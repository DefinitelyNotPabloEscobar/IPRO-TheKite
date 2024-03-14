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

            // Interpolate in local space
            Vector3 localPointOnLine = Vector3.Lerp(Vector3.zero, endTransform.localPosition - startTransform.localPosition, t);

            float curveOffset = Mathf.Sin(t * Mathf.PI) * maxCurveOffset;
            float curveOffsetY = Mathf.Sin(t * Mathf.PI) * maxCurveOffsetY;

            // Apply curve offset in local space
            Vector3 localCurveOffset = new Vector3(0f, curveOffsetY, curveOffset);

            // Convert local point to world space and apply curve offset
            Vector3 pointOnLine = startTransform.position + startTransform.TransformDirection(localPointOnLine + localCurveOffset);

            ropePositions[i] = pointOnLine;
        }

        lineRenderer.SetPositions(ropePositions);
    }

    void LateUpdate()
    {
        DrawCurvedRope();
    }
}

