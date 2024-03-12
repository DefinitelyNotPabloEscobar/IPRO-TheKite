using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class BreathinCyclesFiller : MonoBehaviour
{
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI incorrectText;
    public EndGameMenu endGame;

    private float currentCorrect = 0f;
    private float currentIncorrect = 0f;

    private float correct = 0f;
    private float incorrect = 0f;

    public float seconds = 3f;
    private bool done = false;

    private int CyclesDone;
    void Start()
    {
        AssertCycles();
    }

    private void AssertCycles()
    {
        int inhaleDuration = 0;
        int holdDuration = 0;
        int exhaleDuration = 0;

        int totalCycles = 0;

        CyclesDone = CyclesCompleted.ReadFromFile(SharedConsts.CyclesDonePath);

        switch (Difficulty.ReadFromFile(SharedConsts.DifficultyPath))
        {
            case 0:
            default:

                inhaleDuration = SharedConsts.InhaleTime0;
                holdDuration = SharedConsts.HoldTime0;
                exhaleDuration = SharedConsts.ExhaleTime0;

                break; 

            case 1:

                inhaleDuration = SharedConsts.InhaleTime1;
                holdDuration = SharedConsts.HoldTime1;
                exhaleDuration = SharedConsts.ExhaleTime1;

                break;

            case 2:

                inhaleDuration = SharedConsts.InhaleTime2;
                holdDuration = SharedConsts.HoldTime2;
                exhaleDuration = SharedConsts.ExhaleTime2;

                break;

            case 3:

                inhaleDuration = SharedConsts.InhaleTime3;
                holdDuration = SharedConsts.HoldTime3;
                exhaleDuration = SharedConsts.ExhaleTime3;

                break;
        }

        var extra = SharedConsts.WinTime % (inhaleDuration + holdDuration + exhaleDuration);
        var totalTime = SharedConsts.WinTime + (int)((inhaleDuration + holdDuration + exhaleDuration) - extra);
        totalCycles = (int)totalTime / (inhaleDuration + exhaleDuration + holdDuration) - 1;

        correct = CyclesDone;
        incorrect = totalCycles - CyclesDone;

        correctText.text = "0";
        incorrectText.text = "0";
    }


    IEnumerator MovingNumbers(float seconds)
    {
        var t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            currentCorrect = Mathf.Lerp(0, correct, t / seconds);
            currentIncorrect = Mathf.Lerp(0, incorrect, t / seconds);

            correctText.text = "" + (int)currentCorrect;
            incorrectText.text = "" + (int)currentIncorrect;

            yield return null;
        }
    }

    public void StartAnimation()
    {
        if (done) return;
        StartCoroutine(MovingNumbers(seconds));
        done = true;
    }
}
