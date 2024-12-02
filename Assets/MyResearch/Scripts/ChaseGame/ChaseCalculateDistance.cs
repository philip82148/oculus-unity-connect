using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChaseCalculateDistance : MonoBehaviour
{
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    [Header("Controller Setting")]
    [SerializeField] private ChaseGameController chaseGameController;

    [Header("Calculate Sound")]
    [SerializeField] private ChaseCalculateSound calculateSound;

    [SerializeField] private List<GameObject> targetSoundObjects = new List<GameObject>();

    [SerializeField] private GameObject centralObject;
    private const double requiredLength = 0.03;
    // private const double depthRequiredLength = 0.05;
    [SerializeField] private double centralRequiredLength;
    bool isSound = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isSound = false;
        if (centralObject != null && IsInCentralTerritory())
        {

            // float diff = CalculateControllerPositionAndObjectDiff(centralObject);
            // calculateSound.SetYDiff(diff);
            Vector3 diff = CalculateControllerPositionAndObjectDiffIn3D(centralObject);
            // diffs.Add(diff);
            calculateSound.SetCoordinateDiff(diff);
            isSound = true;
        }
    }


    private bool IsInCentralTerritory()
    {

        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = centralObject.transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < centralRequiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < centralRequiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < centralRequiredLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetRightIndexFingerPosition()
    {
        return indexFinger.transform.position;
    }

    private Vector3 CalculateControllerPositionAndObjectDiffIn3D(GameObject target)
    {
        Vector3 diff = indexFinger.transform.position - target.transform.position;
        return diff;
    }
}
