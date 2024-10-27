using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;
    [Header("Controller Setting")]
    [SerializeField] private DenseSparseExpController denseSparseExpController;

    [Header("Calculate Sound")]
    [SerializeField] private CalculateSound calculateSound;

    [SerializeField] private List<GameObject> targetSoundObjects = new List<GameObject>();
    private GameObject centralObject;
    private const double requiredLength = 0.03;
    private const double depthRequiredLength = 0.05;
    private const double centralRequiredLength = 0.15;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isSound = false;
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Sparse)
        {
            for (int i = 0; i < targetSoundObjects.Count; i++)
            {
                if (IsInSoundTerritory(i))
                {
                    float diff = CalculateControllerPositionAndObjectDiff(targetSoundObjects[i]);
                    calculateSound.SetYDiff(diff);
                    isSound = true;
                }
            }
        }
        else if (denseOrSparse == DenseOrSparse.Dense)
        {
            if (IsInCentralTerritory())
            {

                float diff = CalculateControllerPositionAndObjectDiff(centralObject);
                calculateSound.SetYDiff(diff);
                isSound = true;
            }
        }
        if (!isSound) calculateSound.SetInitial();
    }





    private float CalculateControllerPositionAndObjectDiff(GameObject target)
    {
        float diff = indexFinger.transform.position.y - target.transform.position.y;
        return diff;

    }

    private bool IsInCentralTerritory()
    {

        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = centralObject.transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < centralRequiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < centralRequiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < depthRequiredLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsInSoundTerritory(int index)
    {
        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = targetSoundObjects[index].transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < depthRequiredLength))
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

    public void SetTargetObject(GameObject gameObject)
    {
        targetSoundObjects.Add(gameObject);
    }
    public void SetCentralObject(GameObject gameObject)
    {
        centralObject = gameObject;
    }

    public double GetCentralRequiredLength()
    {
        return centralRequiredLength;
    }

    public double GetRequiredLength()
    {
        return requiredLength;
    }

    public double GetDepthRequiredLength()
    {
        return depthRequiredLength;
    }



}
