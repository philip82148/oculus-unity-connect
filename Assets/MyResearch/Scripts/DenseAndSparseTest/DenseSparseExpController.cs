using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenseSparseExpController : MonoBehaviour
{

    [SerializeField] private GameObject baseObject;
    [SerializeField] private Vector3 startCoordinate;
    [SerializeField] private float interval = 0.05f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;

    private int objectCount = 5;
    // Start is called before the first frame update
    void Start()
    {
        CreateTargetObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void CreateTargetObjects()
    {
        int count = (int)objectCount / 2;

        GameObject gameObject = Instantiate(baseObject, startCoordinate, Quaternion.Euler(0, 0, 0));
        calculateDistance.SetTargetObject(gameObject);

        for (int i = 0; i < count; i++)
        {
            gameObject = Instantiate(baseObject, new Vector3(startCoordinate.x, startCoordinate.y + (i + 1) * interval, startCoordinate.z), Quaternion.Euler(0, 0, 0));
            calculateDistance.SetTargetObject(gameObject);
            gameObject = Instantiate(baseObject, new Vector3(startCoordinate.x, startCoordinate.y - (i + 1) * interval, startCoordinate.z), Quaternion.Euler(0, 0, 0));
            calculateDistance.SetTargetObject(gameObject);
        }
    }


}
