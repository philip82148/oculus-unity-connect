using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeryExpController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private CalculateDistance calculateDistance;

    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();

    [Header("Hand")]
    [SerializeField] private HandController handController;
    [SerializeField] private GameObject targetHand;
    [Header("Setting")]
    [SerializeField] private DenseDataLoggerController dataLoggerController;

    private int targetIndex = 0;

    private bool isGame = true;

    private int score = 0;

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
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            int tmpIndex = handController.GetIndex();
            dataLoggerController.WriteInformation(GetRightIndexFingerPosition());
            if (tmpIndex == -1) return;
            // targetIndex = tmpIndex;
            CheckCorrectAnswer(tmpIndex);
            SetNextTarget();
        }
    }


    private void CheckCorrectAnswer(int tmpIndex)
    {
        if (targetIndex == tmpIndex)
        {
            score += 1;
        }


    }


    private void SetNextTarget()
    {
        int nextIndex = Random.Range(0, targetObjects.Count);
        targetIndex = nextIndex;

        if (isGame)
        {
            dataLoggerController.ReflectPlaceChange(targetIndex);
        }
    }
    private Vector3 GetRightIndexFingerPosition()
    {
        return targetHand.transform.position;
    }
}
