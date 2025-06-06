using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;
    [Header("Controller Setting")]
    [SerializeField] private DenseSparseExpController experimentController;
    [SerializeField] private VRKeyboardExpController vRKeyboardExpController;
    [SerializeField] private SurgeryExpController surgeryExpController;



    [Header("Calculate Sound")]
    [SerializeField] private CalculateSound calculateSound;

    [SerializeField] private List<GameObject> targetSoundObjects = new List<GameObject>();
    [SerializeField] private GameObject centralObject;
    private const double requiredLength = 0.03;
    // private const double depthRequiredLength = 0.05;
    [SerializeField] private double centralRequiredLength;

    [SerializeField] private ExpScene expScene;
    [SerializeField] private DenseOrSparse denseOrSparse;
    // private double xRequiredLength = 0.04;

    // Start is called before the first frame update
    void Start()
    {

        CalculateCentralRequiredLength();

        // denseOrSparse = experimentController.GetDenseOrSparse();
        // expScene = experimentController.GetExpScene();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSound = false;

        List<Vector3> diffs = new List<Vector3>();
        if (denseOrSparse == DenseOrSparse.Sparse)
        {
            if (targetSoundObjects.Count > 0)
            {
                bool isInTerritory = false;
                for (int i = 0; i < targetSoundObjects.Count; i++)
                {
                    if (IsInSoundTerritory(i))
                    {
                        // float diff = CalculateControllerPositionAndObjectDiff(targetSoundObjects[i]);
                        // calculateSound.SetYDiff(diff);


                        Vector3 diff = CalculateControllerPositionAndObjectDiffIn3D(targetSoundObjects[i]);
                        // diffs.Add(diff);
                        calculateSound.SetCoordinateDiff(diff);
                        isInTerritory = true;
                    }
                }
                isSound = isInTerritory;
            }
        }
        else if (denseOrSparse == DenseOrSparse.Dense)
        {
            if (centralObject != null && IsInCentralTerritory())
            {

                // Vector3 diff = CalculateControllerPositionAndObjectDiff(centralObject);
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
        }
        else if (denseOrSparse == DenseOrSparse.NoSound)
        {
            isSound = false;
        }
        if (isSound)
        {
            calculateSound.SetCoordinateDiffs(diffs);
        }
        calculateSound.SetInitial(isSound);
    }


    private Vector3 CalculateControllerPositionAndObjectDiffIn3D(GameObject target)
    {
        Vector3 diff = indexFinger.transform.position - target.transform.position;
        return diff;
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
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < centralRequiredLength))
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
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < requiredLength))
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
    private void CalculateCentralRequiredLength()
    {
        if (expScene == ExpScene.DenseOrSparse)
        {
            centralRequiredLength = (experimentController.GetInterval() * (
                experimentController.GetGridSize() - 1) + 2 * requiredLength) / 2;
        }
        else if (expScene == ExpScene.VRKeyboard)
        {
            centralRequiredLength = vRKeyboardExpController.GetCentralRequiredLength();

        }
        else if (expScene == ExpScene.Surgery)
        {
            centralRequiredLength = surgeryExpController.GetRequiredLength();
        }
    }
    public void SetTargetObject(GameObject gameObject)
    {
        targetSoundObjects.Add(gameObject);
    }
    public void SetCentralObject(GameObject gameObject)
    {
        centralObject = gameObject;
    }
    public void SetNoSound()
    {
        denseOrSparse = DenseOrSparse.NoSound;
    }
    // public double GetXRequiredLength()
    // {
    //     return xRequiredLength;
    // }

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
        // DenseOrSparse denseOrSparse = experimentController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Sparse) return requiredLength;
        else if (denseOrSparse == DenseOrSparse.Dense) return centralRequiredLength;
        else return 0;
    }



}
