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
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public GameObject btn;

    public TextMeshProUGUI hideText;

    public GameObject nextBtn1;
    public GameObject nextBtn2;
    public GameObject nextBtn3;

    public float timeBetweenTexts;
    public float timeWaitBnt;

    private bool Done = false;

    public Image breathBack;
    public RawImage breathGraph;

    
    public void StartAnimation()
    {
        if(Done) return;

        breathBack.enabled = false;
        breathGraph.enabled = false;

        ObjectGroup.SetActive(true);
        mask.enabled = true;

        text1.enabled = true;
        text2.enabled = false;
        text3.enabled = false;
        text4.enabled = false;

        nextBtn1.SetActive(true);
        nextBtn2.SetActive(false);
        nextBtn3.SetActive(false);
        btn.SetActive(false);

        hideText.enabled = false;

        //StartCoroutine(WaitForText(timeBetweenTexts));
        //StartCoroutine(WaitForBtn(timeWaitBnt));

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


    public void Btn2()
    {
        nextBtn1.SetActive(false);
        nextBtn2.SetActive(true);
        text1.enabled = false;
        text2.enabled = true;
    }

    public void Btn3()
    {
        nextBtn2.SetActive(false);
        nextBtn3.SetActive(true);
        text2.enabled = false;
        text3.enabled = true;
    }

    public void Btn4()
    {
        nextBtn3.SetActive(false);
        btn.SetActive(true);
        text3.enabled = false;
        text4.enabled = true;
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
