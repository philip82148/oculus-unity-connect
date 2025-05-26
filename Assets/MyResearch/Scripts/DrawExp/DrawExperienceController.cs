using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Meta.Voice.Samples.BuiltInTimer;
using TMPro;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrawExperienceController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField] private TimeController timeController;

    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;
    [SerializeField] private TargetSpotController targetSpotController;
    [SerializeField] private DrawDataLoggerController drawDataLoggerController;
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;


    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI accuracyScoreText;


    private const int PLACE_COUNT = 9;
    private int targetIndex = 0;
    private int previousIndex = -1;
    private int[] allowedIndexes = { 0, 2, 6, 8 };
    private bool isGame = false;


    // Start is called before the first frame update
    void Start()
    {
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            timeController.StartGameCountDown();
            scoreController.StartGame();
            SetNextTarget();
        }
        if (isGame && OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("isgamestart");
            drawDataLoggerController.WriteCoordinateInformation(targetIndex, indexFinger.transform.position);
        }
    }


    private void SetNextTarget()
    {
        RandomlyChoosePlace();
        ChangeDisplayColor();
        targetSpotController.SetTargetPlaceIndex(targetIndex);

    }


    public void GetCorrectAnswer()
    {
        scoreController.GetCorrectAnswer();
        SetNextTarget();
    }

    public void GetIncorrectAnswer()
    {
        scoreController.GetIncorrectAnswer();
    }

    public void ChangeDisplayColor()
    {
        displayTargetPlaceColorController.ChangeIndexAndReflect(targetIndex);
    }



    public void RandomlyChoosePlace()
    {
        int newIndex;
        do
        {
            newIndex = allowedIndexes[Random.Range(0, allowedIndexes.Length)];
        } while (newIndex == previousIndex);

        previousIndex = newIndex;
        targetIndex = newIndex;
    }

    public void CallGameStart()
    {
        isGame = true;
    }


    public void EndGame()
    {
        scoreController.EndGame();
        float[] resultArray = scoreController.GetFinalResult();
        finalScoreText.text = "Score:" + resultArray[0].ToString("f0");
        accuracyScoreText.text = "Accuracy:" + resultArray[1].ToString("f2");
        drawDataLoggerController.WriteResultInformation(resultArray);
    }

    // public void EndGame1()
    // {
    //     isGame = false;
    //     timeController.EndGame1();
    //     float[] resultArray = scoreController.GetFinalResult();
    //     float consumedTime = timeController.GetConsumedTime();
    //     finalScoreText.text = "Time:" + consumedTime.ToString("f0");
    //     accuracyScoreText.text = "Accuracy:" + resultArray[1].ToString("f2");
    //     drawDataLoggerController.WriteResultInformation(consumedTime, resultArray[1]);


    // }


    private void OnDestroy()
    {
        drawDataLoggerController.Close();
    }
}
