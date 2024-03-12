using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PageSwipperSimpleUp : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float ThresholdOfMovement = 5f;
    public float easing = 0.5f;

    private Vector3 panelLocation;
    private Vector3 initPanelLocation;

    private Vector3 panelLocationMain;
    private Vector3 panelLocationText;

    public Transform MainMenu;
    public Transform Texts;

    private bool isPanelDown = true;

    public PageSwipperSimple swipperHorizontal;
    public bool moving = false;

    private float LowerPanelVerticalSize = 2.25f;
    private float LowerPanelMaxScroll;
    private float LowerPanelMinScroll;
    private float LowerPanelScrollCapUp;
    private float LowerPanelScrollCapDown;

    public CircularStressBarManager circularStressBarManager;


    void Start()
    {
        panelLocation = transform.position;
        panelLocationMain = MainMenu.position;
        panelLocationText = Texts.position;
        initPanelLocation = transform.position;


        if (Screen.height/Screen.width < 1.5)
        {
            LowerPanelVerticalSize = 1.5f;
            LowerPanelMaxScroll = (Screen.height / 2f) - (Screen.height / 2.25f);
            LowerPanelMinScroll = Screen.height / 1.75f;
            LowerPanelScrollCapUp = Screen.height;
            LowerPanelScrollCapDown = -Screen.height / 2;
        }
        else
        {
            LowerPanelMaxScroll = (Screen.height / 2f) - (Screen.height / 2.25f);
            LowerPanelMinScroll = Screen.height / 2.5f;
            LowerPanelScrollCapUp = Screen.height / 2;
            LowerPanelScrollCapDown = -Screen.height / 2;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (swipperHorizontal.moving) return;

        float difference = eventData.pressPosition.y - eventData.position.y;

        if(transform.position.y < initPanelLocation.y + LowerPanelScrollCapUp && transform.position.y > initPanelLocation.y + LowerPanelScrollCapDown)
        {
            transform.position = panelLocation - new Vector3(0, difference, 0);
            MainMenu.position = panelLocationMain -  new Vector3(0, difference, 0);
            Texts.position = panelLocationText -  new Vector3(0, difference, 0);
        }

    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (swipperHorizontal.moving) return;

        if(isPanelDown)
        {
            if (transform.position.y > initPanelLocation.y + LowerPanelMaxScroll)
            {
                panelLocation += new Vector3(0, Screen.height / LowerPanelVerticalSize, 0);
                panelLocationMain += new Vector3(0, Screen.height / LowerPanelVerticalSize, 0);
                panelLocationText += new Vector3(0, Screen.height / LowerPanelVerticalSize, 0);
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                StartCoroutine(SmoothMoveMain(MainMenu.position, panelLocationMain, easing));
                StartCoroutine(SmoothMoveTexts(Texts.position, panelLocationText, easing));
                isPanelDown = false;
                if(circularStressBarManager != null) circularStressBarManager.StartAnimation();
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                StartCoroutine(SmoothMoveMain(MainMenu.position, panelLocationMain, easing));
                StartCoroutine(SmoothMoveTexts(Texts.position, panelLocationText, easing));
            }
        }
        else
        {
            if (transform.position.y <= initPanelLocation.y + LowerPanelMinScroll)
            {
                panelLocation += new Vector3(0, -Screen.height / LowerPanelVerticalSize, 0);
                panelLocationMain += new Vector3(0, -Screen.height / LowerPanelVerticalSize, 0);
                panelLocationText += new Vector3(0, -Screen.height / LowerPanelVerticalSize, 0);
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                StartCoroutine(SmoothMoveMain(MainMenu.position, panelLocationMain, easing));
                StartCoroutine(SmoothMoveTexts(Texts.position, panelLocationText, easing));
                isPanelDown = true;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
                StartCoroutine(SmoothMoveMain(MainMenu.position, panelLocationMain, easing));
                StartCoroutine(SmoothMoveTexts(Texts.position, panelLocationText, easing));
            }
        }
    }


    IEnumerator SmoothMove(Vector3 startPos,  Vector3 endPos, float seconds)
    {
        float t = 0f;
        moving = true;

        while(t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f,1f,t));
            yield return null;
        }

        moving = false;

    }

    IEnumerator SmoothMoveMain(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        moving = true;

        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            MainMenu.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        moving = false;

    }

    IEnumerator SmoothMoveTexts(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        moving = true;

        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            Texts.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        moving = false;

    }


}
