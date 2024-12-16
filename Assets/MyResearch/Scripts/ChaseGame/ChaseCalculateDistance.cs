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
    // [SerializeField] private ChaseCalculateSound calculateSound;
    [SerializeField] private CalculateSound calculateSound;

    [SerializeField] private List<GameObject> targetSoundObjects = new List<GameObject>();

    [SerializeField] private GameObject centralObject;
    private const double requiredLength = 0.03;
    // private const double depthRequiredLength = 0.05;

    [SerializeField] private float xRequiredLength;
    [SerializeField] private float yRequiredLength;
    [SerializeField] private float zRequiredLength;
    bool isSound = false;

    // Start is called before the first frame update
    void Start()
    {
        xRequiredLength = centralObject.transform.localScale.x / 2 * 2;
        yRequiredLength = centralObject.transform.localScale.y / 2 * 3;
        zRequiredLength = centralObject.transform.localScale.z / 2 * 2;
        calculateSound.SetObjectLength(xRequiredLength / 2);
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
        else
        {
            isSound = false;
        }
        calculateSound.SetInitial(isSound);
    }


    private bool IsInCentralTerritory()
    {

        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = centralObject.transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < xRequiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < yRequiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < zRequiredLength))
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
