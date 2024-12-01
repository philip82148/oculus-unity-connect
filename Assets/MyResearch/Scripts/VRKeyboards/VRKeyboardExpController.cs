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

    [SerializeField] private KeyboardHandController handController;
    [SerializeField] private NumberKeyboard numberKeyboard;

    [Header("UI")]

    // [SerializeField] private TextMeshProUGUI restCountText;
    [SerializeField]
    private TextMeshProUGUI ansFirstText;
    [SerializeField] private TextMeshProUGUI ansSecondText;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

    private List<Vector3> targetCoordinates = new List<Vector3>();
    private List<GameObject> targetObjects = new List<GameObject>();

    private int gridSize = 3;

    private int objectCount = 5;
    // private int score = 0;
    private bool isGame = false;
    private int restCount = 11;

    private int targetCorrectIndex = 0;
    private int problemCount = 0;
    private float previousInterval;


    // private int ansFirstIndex = -1;
    // private int ansSecondIndex = -1;

    private const int FIXED_NON_ANSWER_INDEX = -2;



    // Start is called before the first frame update
    void Start()
    {
        objectCount = gridSize * gridSize * gridSize;
        problemCount = numberKeyboard.GetProblemCount();
        previousInterval = interval;
        // CreateTargetObjectsIn3D();
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
            // restCountText.text = "game finished";
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            isGame = true;
            ResetIndex();
            SetNextTarget();
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            // if (ansFirstIndex == FIXED_NON_ANSWER_INDEX)
            // {
            //     ansFirstIndex = handController.GetIndex();
            // }
            // else if (ansSecondIndex == FIXED_NON_ANSWER_INDEX)
            // {
            //     ansSecondIndex = handController.GetIndex();

            //     // 二つの選択肢が揃ったので、答えをチェック
            //     // SetRejoinedIndex();
            // }

            // string

            // ansFirstText.text = "first:" + ansFirstIndex.ToString();
            // ansSecondText.text = "sec:" + ansSecondIndex.ToString();

        }
        if (isGame)
        {
            // restCountText.text = "rest:" + restCount.ToString();
        }


    }

    public void SetNextTarget()
    {
        targetCorrectIndex = Random.Range(0, problemCount);
        GetXYZIndexesForTargetCorrectIndex();
        numberKeyboard.SetNextTargetText(targetCorrectIndex);
        // ChangeDisplayText();
    }

    public void CreateTargetObjectsIn3D()
    {
        int midIndex = gridSize / 2;

        for (int z = 0; z < gridSize; z++) // Zループを外側に
        {
            for (int y = gridSize - 1; y >= 0; y--) // yループを逆順に
            {
                for (int x = 0; x < gridSize; x++) // xループを逆順に
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


                    int index = targetObjects.Count - 1;
                    KeyboardKey keyboardKey = gameObject.GetComponent<KeyboardKey>();
                    keyboardKey.SetIndexes(x, y, z);
                    keyboardKey.SetIndex(index);

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



    public void SetRejoinedIndex()
    {
        CheckCorrectAnswer();
        ResetIndex();
        SetNextTarget();
    }
    private void ResetIndex()
    {
        // ansFirstIndex = FIXED_NON_ANSWER_INDEX;
        // ansSecondIndex = FIXED_NON_ANSWER_INDEX;
    }

    private void CheckCorrectAnswer()
    {
        // bool isCorrect = numberKeyboard.CheckCorrectAnswer(ansFirstIndex, ansSecondIndex);

    }



    private void GetXYZIndexesForTargetCorrectIndex()
    {
        int xIndex = gridSize - 1 - (targetCorrectIndex % gridSize);
        int yIndex = gridSize - 1 - ((targetCorrectIndex / gridSize) % gridSize);
        int zIndex = targetCorrectIndex / (gridSize * gridSize);

        Debug.Log($"TargetCorrectIndex: {targetCorrectIndex}, x: {xIndex}, y: {yIndex}, z: {zIndex}");
    }





    /// <summary>
    /// ここから下はいったんいらない
    /// </summary>
    /// <returns></returns>

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

    public void DestoryKeyboard()
    {
        for (int i = 0; i < targetObjects.Count; i++)
            Destroy(targetObjects[i]);

    }
}
