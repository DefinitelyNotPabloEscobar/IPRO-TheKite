using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float followTargetInSeconds = 10;
    public float lerpTime;
    public float _counter = 0;
    public float _lerpCounter = 0;

    private void Update()
    {
        if (_counter < followTargetInSeconds)
        {
            _counter += Time.deltaTime;
            return;
        }
        _lerpCounter += _lerpCounter > lerpTime ? 0 : Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, _lerpCounter * Time.deltaTime);

        Quaternion OriginalRot = transform.rotation;
        transform.LookAt(target);
        Quaternion NewRot = transform.rotation;
        transform.rotation = OriginalRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, _lerpCounter * Time.deltaTime);
    }
}
