using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeryExpController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private CalculateDistance calculateDistance;

    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            calculateDistance.SetTargetObject(targetObjects[i]);
        }
        // int centralIndex = (int)targetObjects.Count / 2;
        // calculateDistance.SetCentralObject(targetObjects[centralIndex]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
