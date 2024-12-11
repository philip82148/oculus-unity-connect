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
    [SerializeField] private float centralRequiredLength = 0.06f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;

    [SerializeField] private KeyboardHandController handController;
    [SerializeField] private NumberKeyboard numberKeyboard;

    [Header("UI")]

    // [SerializeField] private TextMeshProUGUI restCountText;
    [SerializeField]
    private TextMeshProUGUI ansFirstText;
    [SerializeField] private TextMeshProUGUI ansSecondText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

    private List<Vector3> targetCoordinates = new List<Vector3>();
    private List<GameObject> targetObjects = new List<GameObject>();

    private int gridSize = 3;

    private int objectCount = 5;
    // private int score = 0;
    private bool isGame = false;
    // private int restCount = 11;

    private int targetCorrectIndex = 0;
    private int problemCount = 0;
    private float previousInterval;


    // private int ansFirstIndex = -1;
    // private int ansSecondIndex = -1;

    private const int FIXED_NON_ANSWER_INDEX = -2;
    private float gameTime = 60f;


    // Start is called before the first frame update
    void Start()
    {
        objectCount = gridSize * gridSize * gridSize;
        problemCount = numberKeyboard.GetProblemCount();
        previousInterval = centralRequiredLength;
        // CreateTargetObjectsIn3D();
    }

    // Update is called once per frame
    void Update()
    {
        if (centralRequiredLength != previousInterval)
        {
            // UpdateObjectPositionsIn3D();
            previousInterval = centralRequiredLength;
        }
        // if (restCount <= 0)
        // {
        //     isGame = false;
        //     // restCountText.text = "game finished";
        // }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            isGame = true;
            numberKeyboard.ResetScore(); // スコアのリセット
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
            UpdateTimer();
            // restCountText.text = "rest:" + restCount.ToString();
        }


    }

    private void UpdateTimer()
    {
        if (!isGame) return;
        gameTime -= Time.deltaTime;

        // タイマー表示の更新
        timerText.text = "Time: " + Mathf.Clamp(gameTime, 0, 999).ToString("F2");

        if (gameTime <= 0)
        {
            gameTime = 0;
            isGame = false;
            EndGame();
        }
    }
    private void EndGame()
    {
        isGame = false;
        // ゲームオーバーの表示やスコアの表示など
        timerText.text = "Time: 0.00";
        // Debug.Log("Game Over! Final Score: " + numberKeyboard.GetScore());
        // 必要に応じて他のUIを更新
    }
    public void SetNextTarget()
    {
        targetCorrectIndex = Random.Range(0, problemCount);
        GetXYZIndexesForTargetCorrectIndex();
        numberKeyboard.SetNextTargetText(targetCorrectIndex);
        // ChangeDisplayText();
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

    public float GetCentralRequiredLength()
    {
        return centralRequiredLength;
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
