using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Meta.Voice.Samples.BuiltInTimer;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    // [SerializeField] private AudioSettingController audioSettingController;


    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI accuracyScoreText;





    private const int PLACE_COUNT = 9;
    private int targetIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        SetNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            timeController.StartGameCountDown();
            scoreController.StartGame();
        }
    }


    private void SetNextTarget()
    {
        RandomlyChoosePlace();
        ChangeDisplayColor();
        targetSpotController.SetTargetPlaceIndex(targetIndex);
        // audioSettingController.SetTargetPlaceIndex(targetIndex);
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
        targetIndex = Random.Range(0, PLACE_COUNT);
    }
    // public void RandomlyChoosePlace()
    // {
    //     int[] allowedIndexes = { 0, 3, 4, 9 };
    //     targetIndex = allowedIndexes[Random.Range(0, allowedIndexes.Length)];
    // }

    public void EndGame()
    {
        scoreController.EndGame();
        float[] resultArray = scoreController.GetFinalResult();
        finalScoreText.text = "Score:" + resultArray[0].ToString("f0");
        accuracyScoreText.text = "Accuracy:" + resultArray[1].ToString("f2");
        drawDataLoggerController.WriteResultInformation(resultArray);
    }

    private void OnDestroy()
    {
        drawDataLoggerController.Close();
    }
}
