using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwipper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    public List<GameObject> pages;

    public Button fowardBtn;
    public Button backBtn;

    public Button skipBtn;
    public TextMeshProUGUI skipText;
    public Button practiceBtn;
    public TextMeshProUGUI practiceText;

    private Vector3 panelLocation;

    public List<GameObject> lowerIconLight;
    public List<GameObject> lowerIconDark;

    public VideoManager videoManager;
    public int videoPageNumber;
    public IconViewManagerPlayAndPause iconViewManager;

    private int currentPage;

    void Start()
    {
        if (pages == null || pages.Count < 1) {
            Debug.Log("Error: Pages must bot be null nor less than 1.");
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.position = new Vector3(
                pages[0].transform.position.x + Screen.width * i,
                pages[0].transform.position.y, 
                0f);
        }

        panelLocation = transform.position;

        SwippeBtnChecker();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector3 (difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (pages == null || pages.Count < 1)
        {
            Debug.Log("Error: Pages must bot be null nor less than 1.");
            return;
        }

        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold) 
        {
            if (percentage > 0)
            {
                GoFoward();
            }
            else if(percentage < 0)
            {
                GoBack();
            }
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }

    }


    IEnumerator SmoothMove(Vector3 startPos,  Vector3 endPos, float seconds)
    {
        SwippeBtnChecker();

        float t = 0f;

        while(t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f,1f,t));
            yield return null;
        }

    }

    public void SwippeBtnChecker()
    {
        if (currentPage == 0)
        {
            backBtn.enabled = false;
            backBtn.image.enabled = false;

            switchToSkipBtn();
        }
        else if (currentPage >= pages.Count - 1)
        {
            fowardBtn.enabled = false;
            fowardBtn.image.enabled = false;

            switchToPracticeBtn();
        }
        else
        {
            backBtn.enabled = true;
            backBtn.image.enabled = true;
            fowardBtn.enabled = true;
            fowardBtn.image.enabled = true;

            switchToSkipBtn();
        }
    }

    public void SwipperSkipBtnsUpdate()
    {
        backBtn.enabled = true;
        backBtn.image.enabled = true;
        fowardBtn.enabled = false;
        fowardBtn.image.enabled = false;

        lowerIconLight[currentPage].SetActive(false);
        lowerIconDark[currentPage].SetActive(true);

    }

    public void AddLightIcons(List<GameObject> list)
    {
        this.lowerIconLight = list;
    }

    public void AddDarkIcons(List<GameObject> list)
    {
        this.lowerIconDark = list;
    }

    public void GoFoward()
    {
        Vector3 newLocation = panelLocation;
        if (currentPage < pages.Count - 1)
        {
            newLocation += new Vector3(-Screen.width, 0, 0);
            currentPage += 1;

            lowerIconLight[currentPage].SetActive(false);
            lowerIconDark[currentPage].SetActive(true);

            lowerIconLight[currentPage - 1].SetActive(true);
            lowerIconDark[currentPage - 1].SetActive(false);
        }
        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
        panelLocation = newLocation;

        manageVideoPage();
    }

    public void GoBack()
    {
        Vector3 newLocation = panelLocation;
        if (currentPage > 0)
        {
            newLocation += new Vector3(Screen.width, 0, 0);
            currentPage -= 1;

            lowerIconLight[currentPage].SetActive(false);
            lowerIconDark[currentPage].SetActive(true);

            lowerIconLight[currentPage + 1].SetActive(true);
            lowerIconDark[currentPage + 1].SetActive(false);
        }

        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
        panelLocation = newLocation;

        manageVideoPage();
    }

    public void manageVideoPage()
    {
        if (currentPage == videoPageNumber)
        {
            videoManager.PlayVideo();
            iconViewManager.iconPlay.Appear();
            skipBtn.enabled = false;
            skipBtn.image.enabled = false;
            skipText.enabled = false;
        }
        else if (currentPage == videoPageNumber - 1 || currentPage == videoPageNumber + 1)
        {
            skipBtn.enabled = true;
            skipBtn.image.enabled = true;
            skipText.enabled = true;
            videoManager.StopVideo();
        }
        else
        {
            videoManager.StopVideo();
        }
    }

    public void switchToPracticeBtn()
    {
        skipBtn.enabled = false;
        skipBtn.image.enabled = false;
        skipText.enabled = false;

        practiceBtn.enabled = true;
        practiceBtn.image.enabled = true;
        practiceText.enabled = true;
    }

    public void switchToSkipBtn()
    {
        skipBtn.enabled = true;
        skipBtn.image.enabled = true;
        skipText.enabled = true;

        practiceBtn.enabled = false;
        practiceBtn.image.enabled = false;
        practiceText.enabled = false;
    }

    public void Skip()
    {
        //currentPage = pages.Count - 1;
        //panelLocation = pages[pages.Count - 1].transform.position;

        lowerIconLight[currentPage].SetActive(true);
        lowerIconDark[currentPage].SetActive(false);


        while (currentPage < pages.Count - 1)
        {
            panelLocation += new Vector3(-Screen.width, 0, 0);
            currentPage += 1;
        }

        lowerIconLight[currentPage].SetActive(true);
        lowerIconDark[currentPage].SetActive(false);

        StartCoroutine(SmoothMove(transform.position, panelLocation, 3 * easing));

        SwipperSkipBtnsUpdate();

        manageVideoPage();

    }



}
