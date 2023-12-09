using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public KiteMovementScript kite;
    public Transform referencePosition;

    public float minScale = 0.75f;
    public float maxScale = 1.25f;
    public float maxProx = 0.85f;
    public float minProx = 1.15f;
    public int numberOfPrefabs;
    public float offset = 10f;

    private float radius;

    void Start()
    {
        if(kite != null && referencePosition != null)
        {
            radius = kite.transform.position.z + offset;
            Generate();
        }
        
    }
    void Generate()
    {
        {
            if (prefabs == null || prefabs.Length == 0)
            {
                Debug.LogError("Error, no prefabs assigned to the PrefabManager.");
                return;
            }

            float angleIncrement = 360f / numberOfPrefabs;
            for (int i = 0; i < numberOfPrefabs; i++)
            {
                float randomRadiusFactor = Random.Range(minProx, maxProx);
                float adjustedRadius = radius * randomRadiusFactor;
                Vector3 spawnPosition = GetCirclePosition(i * angleIncrement, adjustedRadius);

                Quaternion spawnRotation = Quaternion.identity;
                GameObject selectedPrefab = GetRandomPrefab();

                float randomScale = Random.Range(minScale, maxScale);
                selectedPrefab.transform.localScale = new Vector3(randomScale*2, randomScale*2, randomScale*2);

                Instantiate(selectedPrefab, spawnPosition, spawnRotation, transform);
            }
        }
    }

    Vector3 GetCirclePosition(float angle, float radius)
    {
        float radians = Mathf.Deg2Rad * angle;
        float x = referencePosition.position.x + radius * Mathf.Cos(radians);
        float z = referencePosition.position.z + radius * Mathf.Sin(radians);

        return new Vector3(x, transform.position.y, z);
    }

    GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }



}
