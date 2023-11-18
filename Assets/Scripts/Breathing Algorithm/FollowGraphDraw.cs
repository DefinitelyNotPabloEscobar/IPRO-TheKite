using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGraphDraw : MonoBehaviour
{
    public Vector2 offset;
    public Transform target;

    private void Update()
    {
        transform.position = new Vector3(target.position.x + offset.x, transform.position.y + offset.y, -10);
    }
}
