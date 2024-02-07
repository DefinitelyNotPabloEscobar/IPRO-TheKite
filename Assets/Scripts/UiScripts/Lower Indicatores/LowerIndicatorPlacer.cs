using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LowerIndicatorPlacer : MonoBehaviour
{
    public GameObject ellipse;
    public GameObject objGroup;

    public int number = 0;
    public float spacing = 0.05f;

    public bool darker = false;

    public PageSwipper pageSwipper;

    private List<GameObject> gameObjectList = new List<GameObject>();

    void Awake()
    {
        if (number % 2 == 0) Par(number);
        else Impar();

        Destroy(ellipse);

        gameObjectList = SortGameObjectsByX(gameObjectList);

        if (!darker)
        {
            gameObjectList[0].SetActive(false);
            pageSwipper.AddLightIcons(gameObjectList);
        }
        else
        {
            gameObjectList[0].SetActive(true);
            pageSwipper.AddDarkIcons(gameObjectList);
        }

        Debug.Log("Screeen " + Screen.width);
        if (Screen.width < 1500)
        {
            spacing = 0.001f;
        }
    }

    private void Impar()
    {
        var obj = Instantiate(ellipse, ellipse.transform.position, Quaternion.identity, objGroup.transform);
        obj.SetActive(!darker);
        gameObjectList.Add(obj);

        int parNumber = number - 1;
        
        Par(parNumber);
    }

    private void Par(int n)
    {
        int parNumber = n / 2;
        float originalX = ellipse.transform.position.x;
        float originalY = ellipse.transform.position.y;
        float originalZ = ellipse.transform.position.z;


        for (int i = 0; i < parNumber; i++)
        {
            Vector3 position = new Vector3(originalX - (spacing * Screen.width * (i + 1)), originalY, originalZ);
            var obj = Instantiate(ellipse, position, Quaternion.identity, objGroup.transform);
            obj.SetActive(!darker);
            gameObjectList.Add(obj);
        }

        for (int i = 0; i < parNumber; i++)
        {
            Vector3 position = new Vector3(originalX + (spacing * Screen.width * (i + 1)), originalY, originalZ);
            var obj = Instantiate(ellipse, position, Quaternion.identity, objGroup.transform);
            obj.SetActive(!darker);
            gameObjectList.Add(obj);
        }
    }

    public List<GameObject> SortGameObjectsByX(List<GameObject> objectsToSort)
    {
        List<GameObject> sortedObjects = objectsToSort.OrderBy(obj => obj.transform.position.x).ToList();

        return sortedObjects;
    }

}
