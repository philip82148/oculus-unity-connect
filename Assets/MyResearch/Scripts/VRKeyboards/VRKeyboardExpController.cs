using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Body.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class VRKeyboardExpController : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private Vector3 startCoordinate;
    [SerializeField] private float interval = 0.07f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;
    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;
    [SerializeField] private TargetDisplayTextController targetDisplayTextController;
    [SerializeField] private HandController handController;
    [SerializeField] private NumberKeyboard numberKeyboard;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI restCountText;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (interval != previousInterval)
        {
            UpdateObjectPositionsIn3D();
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

        for (int z = 0; z < gridSize; z++) // Zループを外側に
        {
            for (int y = gridSize - 1; y >= 0; y--) // yループを逆順に
            {
                for (int x = gridSize - 1; x >= 0; x--) // xループを逆順に
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

                    // // 追加のセットアップ
                    // TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
                    // if (text != null)
                    // {
                    //     text.text = targetObjects.Count.ToString();
                    // }
                    // else
                    // {
                    //     Debug.Log("text null");
                    // }

                    int index = targetObjects.Count - 1;
                    KeyboardKey keyboardKey = gameObject.GetComponent<KeyboardKey>();
                    keyboardKey.SetIndexes(x, y, z);
                    // keyboardKey.SetIndex(index);

                    calculateDistance.SetTargetObject(gameObject);
                    if (x == midIndex && y == midIndex && z == midIndex)
                    {
                        calculateDistance.SetCentralObject(gameObject);
                    }
                }
            }
        }
    }

    private void UpdateObjectPositionsIn3D()
    {
        int midIndex = gridSize / 2;
        int index = 0;

        for (int z = 0; z < gridSize; z++)
        {
            for (int y = gridSize - 1; y >= 0; y--)
            {
                for (int x = gridSize - 1; x >= 0; x--)
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


    public bool GetIsGame()
    {
        return isGame;
    }


    private void SetNumberKeyboard()
    {

    }

    private void DecideTargetIndex()
    {
        targetCorrectIndex = Random.Range(0, objectCount);
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
        int xIndex = gridSize - 1 - (targetCorrectIndex % gridSize);
        int yIndex = gridSize - 1 - ((targetCorrectIndex / gridSize) % gridSize);
        int zIndex = targetCorrectIndex / (gridSize * gridSize);

        Debug.Log($"TargetCorrectIndex: {targetCorrectIndex}, x: {xIndex}, y: {yIndex}, z: {zIndex}");
    }

    public void ChangeDisplayText()
    {
        int xIndex = gridSize - 1 - (targetCorrectIndex % gridSize);
        int yIndex = gridSize - 1 - ((targetCorrectIndex / gridSize) % gridSize);
        int zIndex = targetCorrectIndex / (gridSize * gridSize);

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
}
