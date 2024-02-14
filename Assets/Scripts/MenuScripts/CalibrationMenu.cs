using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrationMenu : MonoBehaviour
{
    public GameObject ObjectGroup;
    public Image mask;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public GameObject btn;

    public TextMeshProUGUI hideText;

    public float timeBetweenTexts;
    public float timeWaitBnt;

    private bool Done = false;

    
    public void StartAnimation()
    {
        if(Done) return;

        ObjectGroup.SetActive(true);
        mask.enabled = true;
        text1.enabled = true;
        text2.enabled = false;
        btn.SetActive(false);
        hideText.enabled = false;

        StartCoroutine(WaitForText(timeBetweenTexts));
        StartCoroutine(WaitForBtn(timeWaitBnt));

        Done = true;
    }


    IEnumerator WaitForText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        text1.enabled = false;
        text2.enabled = true;
    }

    IEnumerator WaitForBtn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        btn.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(SharedConsts.BeforePlayMenu);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SharedConsts.Breath);
    }

    public void Leave()
    {
        SceneManager.LoadScene(SharedConsts.StartingMenu);
    }



}
