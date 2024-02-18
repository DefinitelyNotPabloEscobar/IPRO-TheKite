using System;
using System.Collections;
using System.Linq;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class GraphDraw : MonoBehaviour {
    [Header ("Objects")]
    public Transform patternControlled;
    public Transform userControlled;
    public Transform breath;

    [Header ("Settings")]
    public float moveSpeed;
    public float frequency;
    public float amplitude;
    public float userAmplitude;
    public float breathAmplitude = 64;
    public float[] envelope = new float[2500];
    public int seconds = 4;
    public float delay = 1f;

    /*------------------------------------*/

    private Vector3 _userPos, _patternPos, _breathPos;
    private Vector3 _patternInitialPos;
    private float _currentFrequency;
    private float _phase = 0;
    private float _elapsedTime;
    private float _snapshot = 0f;
    private bool _snapshotFlag = false;
    private int _positionChangeCounter = 0;
    private int _interpResolution = 20;

    private int fps = 0, counter = 0;
    private float time = 0;
    private bool firstTime = true;

    internal bool show = false;

    private bool flag = true;
    private bool flag1 = true;
    
    private bool calibrateMicMode = true;
    /*------------------------------------*/

    private void Start () {

        _userPos = userControlled.position;
        _currentFrequency = frequency;
        _patternInitialPos = patternControlled.position;
        _patternPos = patternControlled.position;
        _elapsedTime = 0.0f;

        _breathPos = breath.position;
        _breathPos.x = _patternPos.x;

    }
    public void enableMicTest()
    {
        calibrateMicMode = true;
    }
    public void disableMicTest()
    {
        calibrateMicMode = false;
    }

    void Update () {
        time += Time.deltaTime;
        counter += 1;

        if (time <= seconds && firstTime)
        {
            fps += 1;
            return;
        }
        if (time > seconds && firstTime)
        {
            firstTime = false;
            fps /= seconds;
        }

        if (flag1)
        {
            if (time <= seconds + 1)
            {
                return;
            }
            else
            {
                flag1 = false;
                counter = 0;
                time = 0;
            }
        }

        if (time >= 1 )
        {
            time = 0;
            counter = 0;
        }

        if (!show) return;

        if (frequency != _currentFrequency)
            ChangeFrequency ();

        _elapsedTime += Time.deltaTime;

        _userPos += transform.right * moveSpeed * Time.deltaTime;
        _patternPos += transform.right * moveSpeed * Time.deltaTime;

        Vector3 temp = _patternPos;
        try
        {
            temp = ComputeBreathPos();
        }
        catch (InvalidOperationException ex)
        {
            //Debug.Log(ex.Message);
        }

        /*
        if (!calibrateMicMode){
            breath.gameObject.SetActive(true);
            breath.position = Vector3.Lerp(breath.position, temp, .01f);
            breath.position = new Vector3(_patternPos.x, breath.position.y, breath.position.z);
        }
        else{
            breath.gameObject.SetActive(false);
        }
        */

        breath.gameObject.SetActive(true);
        breath.position = Vector3.Lerp(breath.position, temp, .01f);
        breath.position = new Vector3(_patternPos.x, breath.position.y, breath.position.z);


        patternControlled.position = Vector3.Lerp (patternControlled.position, ComputePositionSin (1), .5f);
            //userControlled.position = Vector3.Lerp(userControlled.position, UserComputePositionSin(_waveSelection.sign), .5f);
        userControlled.position = UpdateUserPositions (UserComputePositionSin);

        //print(fps);
        //print(envelope.Length);
    }

    private Vector3 ComputeBreathPos()
    {
        var temp = envelope.Skip(counter * 2500 / fps);
        var temp2 = temp.Take(2500 / fps);
        var temp3 = temp2.ToArray().Average();
        return _breathPos + transform.up * temp3 * breathAmplitude;
    }

    private Vector3 UpdateUserPositions (Func<float, Vector3> func) {
        double[] xs = { 0, .25f, .75f, 1 };

        double[] uX = {
            userControlled.position.x,
            Vector3.Lerp (userControlled.position, func (1), .25f).x,
            Vector3.Lerp (userControlled.position, func (1), .75f).x,
            func (1).x
        };

        double[] uY = {
            userControlled.position.y,
            Vector3.Lerp (userControlled.position, func (1), .25f).y,
            Vector3.Lerp (userControlled.position, func (1), .75f).y,
            func (1).y
        };

        double[] uZ = {
            userControlled.position.z,
            Vector3.Lerp (userControlled.position, func (1), .25f).z,
            Vector3.Lerp (userControlled.position, func (1), .75f).z,
            func (1).z
        };

        var newUX = new ScottPlot.Statistics.Interpolation.NaturalSpline (xs, uX, _interpResolution);
        var newUY = new ScottPlot.Statistics.Interpolation.NaturalSpline (xs, uY, _interpResolution);
        var newUZ = new ScottPlot.Statistics.Interpolation.NaturalSpline (xs, uZ, _interpResolution);

        if (_positionChangeCounter == _interpResolution) _positionChangeCounter = 0;

        float finaluX = (float) newUX.interpolatedYs[_positionChangeCounter];
        float finaluY = (float) newUY.interpolatedYs[_positionChangeCounter];
        float finaluZ = (float) newUZ.interpolatedYs[_positionChangeCounter];
        _positionChangeCounter++;

        //Debug.Log(new Vector3(finaluX, finaluY, finaluZ));

        return new Vector3 (finaluX, finaluY, finaluZ);
    }

    private void ChangeFrequency () {
        float curr = (_elapsedTime * _currentFrequency + _phase) % (2.0f * Mathf.PI);
        float next = (_elapsedTime * frequency) % (2.0f * Mathf.PI);
        _phase = curr - next;
        _currentFrequency = frequency;
        //Debug.Log ($"current freq: {_currentFrequency}.. phase: {_phase}");
    }

    private Vector3 ComputePositionCos (float sign) {
        return _patternPos + transform.up * sign * Mathf.Cos (_elapsedTime * _currentFrequency + _phase) * amplitude;
    }

    private Vector3 ComputePositionSin (float sign) {
        return _patternPos + transform.up * sign * Mathf.Sin (_elapsedTime * _currentFrequency + _phase) * amplitude;
    }

    private Vector3 UserComputePositionCos (float sign) {
        return _patternPos + transform.up * sign * Mathf.Cos (_elapsedTime * _currentFrequency + _phase) * userAmplitude;
    }

    private Vector3 UserComputePositionSin (float sign) {
        return _patternPos + transform.up * sign * Mathf.Sin (_elapsedTime * _currentFrequency + _phase) * userAmplitude;
    }

    public void CreateSnapshot () {
        if (_snapshotFlag) return;
        amplitude = 0f;
        _snapshot = _elapsedTime;
        _snapshotFlag = true;
        //Debug.Log ($"Snapshot time: {_elapsedTime}");
    }

    public void ReturnToSnapshot () {
        if (!_snapshotFlag) return;
        amplitude = 4f;
        _elapsedTime = _snapshot;
        _snapshotFlag = false;
        //Debug.Log ($"Returning to time: {_elapsedTime}");
    }

    //double the period should be passed to this function
    //we are using the upper half of the wave for the inhale state and the lower half for the exhale state
    public void SetFrequency (float period) {
        frequency = Mathf.PI / period;
    }
    public Transform GetUserControlled () {
        return userControlled;
    }
    public Transform GetPatternControlled () {
        return patternControlled;
    }
    public Transform GetBreath () {
        return breath;
    }
    public float GetDelay() {
        return delay;
    }
}