using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Body.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DenseSparseExpController : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private Vector3 startCoordinate;
    [SerializeField] private float interval = 0.07f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;
    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;
    [SerializeField] private TargetDisplayTextController targetDisplayTextController;
    [SerializeField] private HandController handController;
    [SerializeField] private DenseDataLoggerController dataLoggerController;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI restCountText;
    [Header("Hand object")]
    [SerializeField] private GameObject targetHand;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

    [SerializeField] private DenseOrSparse denseOrSparse;
    [SerializeField]
    private ExpScene expScene = ExpScene.DenseOrSparse;


    private List<Vector3> targetCoordinates = new List<Vector3>();
    private List<GameObject> targetObjects = new List<GameObject>();

    private int gridSize = 3;

    private int objectCount = 5;
    private int score = 0;
    private bool isGame = false;
    private int restCount = 11;

    private int targetCorrectIndex = 0;
    private float previousInterval;



    // Start is called before the first frame update
    void Start()
    {
        objectCount = gridSize * gridSize * gridSize;
        previousInterval = interval;
        CreateTargetObjectsIn3D();
        dataLoggerController.Initialize(interval, denseOrSparse);
    }

    // Update is called once per frame
    void Update()
    {
        if (interval != previousInterval)
        {
            UpdateObjectPositionsIn3D();
            // UpdateObjectPositions();
            previousInterval = interval;
        }
        if (restCount <= 0)
        {
            isGame = false;
            restCountText.text = "game finished";
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            isGame = true;

            SetNextTarget();
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            int tmpIndex = handController.GetIndex();
            if (isGame)
            {
                dataLoggerController.WriteInformation(GetRightIndexFingderPosition());
            }
            SetRejoinedIndex(tmpIndex);
        }
        if (isGame)
        {
            scoreText.text = "score:" + score.ToString();
            restCountText.text = "rest:" + restCount.ToString();
        }
    }

    public void SetNextTarget()
    {
        restCount -= 1;
        DecideTargetIndex();
        ChangeDisplayColor();
    }

    private void CreateTargetObjectsIn3D()
    {
        int midIndex = gridSize / 2;


        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {

                    float positionOffsetX = (x - midIndex) * interval;
                    float positionOffsetY = (y - midIndex) * interval;
                    float positionOffsetZ = (z - midIndex) * interval;
                    Vector3 newPosition = new Vector3(
                                        startCoordinate.x + positionOffsetX,
                                        startCoordinate.y + positionOffsetY,
                                        startCoordinate.z + positionOffsetZ);
                    GameObject gameObject = Instantiate(baseObject, newPosition, Quaternion.identity);
                    targetObjects.Add(gameObject);
                    targetCoordinates.Add(newPosition);

                    // 追加のセットアップ
                    TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
                    if (text != null)
                    {
                        text.text = (targetObjects.Count).ToString();
                    }
                    else
                    {
                        Debug.Log("text null");
                    }

                    int index = targetObjects.Count - 1;
                    PaletteObjectController paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
                    paletteObjectController.SetIndexes(x, y, z);
                    paletteObjectController.SetIndex(index);

                    calculateDistance.SetTargetObject(gameObject);
                    if (x == midIndex && y == midIndex && z == midIndex)
                    {
                        calculateDistance.SetCentralObject(gameObject);
                    }
                }
            }
        }
    }

    private void CreateTargetObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            float positionOffset = 0f;
            if (objectCount % 2 == 1)
            {
                // Odd number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex) * interval; // Reversed the sign
            }
            else
            {
                // Even number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex + 0.5f) * interval; // Reversed the sign
            }

            Vector3 newPosition = new Vector3(startCoordinate.x, startCoordinate.y + positionOffset, startCoordinate.z);
            GameObject gameObject = Instantiate(baseObject, newPosition, Quaternion.identity);
            targetObjects.Add(gameObject);
            targetCoordinates.Add(newPosition);

            // Additional setup
            TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
            if (text != null)
            {
                text.text = (i + 1).ToString();
            }
            else
            {
                Debug.Log("text null");
            }

            PaletteObjectController paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
            paletteObjectController.SetIndex(i);

            calculateDistance.SetTargetObject(gameObject);
            if (i == objectCount / 2)
            {
                calculateDistance.SetCentralObject(gameObject);
            }
        }
    }
    private void UpdateObjectPositionsIn3D()
    {
        int gridSize = 3; // グリッドのサイズ（3×3×3）
        int midIndex = gridSize / 2; // 中央のインデックス（1）
        int index = 0;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    float positionOffsetX = (x - midIndex) * interval;
                    float positionOffsetY = (y - midIndex) * interval;
                    float positionOffsetZ = (z - midIndex) * interval;

                    Vector3 newPosition = new Vector3(
                        startCoordinate.x + positionOffsetX,
                        startCoordinate.y + positionOffsetY,
                        startCoordinate.z + positionOffsetZ);

                    targetObjects[index].transform.position = newPosition;
                    targetCoordinates[index] = newPosition;
                    index++;
                }
            }
        }
    }
    private void UpdateObjectPositions()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            float positionOffset = 0f;
            if (objectCount % 2 == 1)
            {
                // Odd number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex) * interval; // Reversed the sign
            }
            else
            {
                // Even number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex + 0.5f) * interval; // Reversed the sign
            }

            Vector3 newPosition = new Vector3(startCoordinate.x, startCoordinate.y + positionOffset, startCoordinate.z);
            targetObjects[i].transform.position = newPosition;
            targetCoordinates[i] = newPosition;
        }
    }

    public DenseOrSparse GetDenseOrSparse()
    {
        return denseOrSparse;
    }

    public bool GetIsGame()
    {
        return isGame;
    }

    private void DecideTargetIndex()
    {
        targetCorrectIndex = Random.Range(0, objectCount);
        dataLoggerController.ReflectPlaceChange(targetCorrectIndex);
        GetXYZIndexesForTargetCorrectIndex();
        ChangeDisplayText();
    }

    public void SetRejoinedIndex(int rejoinedIndex)
    {
        CheckCorrectAnswer(rejoinedIndex);
        DecideTargetIndex();
        SetNextTarget();
    }

    private void CheckCorrectAnswer(int rejoinedIndex)
    {
        if (targetCorrectIndex == rejoinedIndex)
        {
            score += 1;
        }
    }

    public void ChangeDisplayColor()
    {
        displayTargetPlaceColorController.ChangeIndexAndReflect(targetCorrectIndex);
    }
    private void GetXYZIndexesForTargetCorrectIndex()
    {
        int xIndex = targetCorrectIndex / (gridSize * gridSize);
        int yIndex = (targetCorrectIndex / gridSize) % gridSize;
        int zIndex = targetCorrectIndex % gridSize;

        Debug.Log($"TargetCorrectIndex: {targetCorrectIndex}, x: {xIndex}, y: {yIndex}, z: {zIndex}");
    }

    public void ChangeDisplayText()
    {
        int xIndex = targetCorrectIndex / (gridSize * gridSize);
        int yIndex = (targetCorrectIndex / gridSize) % gridSize;
        int zIndex = targetCorrectIndex % gridSize;

        targetDisplayTextController.SetTargetXYZ(xIndex, yIndex, zIndex);

    }

    public Vector3 GetStartCoordinate()
    {
        return startCoordinate;
    }

    public List<Vector3> GetTargetCoordinates()
    {
        return targetCoordinates;
    }

    public float GetInterval()
    {
        return interval;
    }
    public int GetObjectCount()
    {
        return objectCount;
    }
    public int GetGridSize()
    {
        return gridSize;
    }
    private Vector3 GetRightIndexFingderPosition()
    {
        return targetHand.transform.position;
    }
    private void OnDestroy()
    {
        dataLoggerController.Close();
    }
}

public enum DenseOrSparse
{
    Dense,
    Sparse
}

public enum ExpScene
{
    DenseOrSparse,
    VRKeyboard
}