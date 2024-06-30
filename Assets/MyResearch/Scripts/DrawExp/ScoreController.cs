using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Oculus.Platform;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField]
    private DrawExperienceController drawExperienceController;
    [Header("UI Setting")]

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI incorrectCountText;
    [SerializeField]
    private TextMeshProUGUI totalCountText;

    private int totalCount = 0;
    private int correctCount = 0;
    private int incorrectCount = 0;

    private bool isGame = true;

    private const int requiredScore = 10;


    void Initialize()
    {
        correctCount = 0;
        totalCount = 0;
        incorrectCount = 0;
    }

    void Update()
    {
        scoreText.text = correctCount.ToString();
        incorrectCountText.text = "false:" + incorrectCount.ToString();
        totalCountText.text = "total:" + totalCount.ToString();
    }


    public void StartGame()
    {
        Initialize();
    }

    public void GetCorrectAnswer()
    {
        if (isGame)
        {
            AddScore();
            AddTotalCount();
            CheckIfGameFinishScore();
        }
    }
    public void GetIncorrectAnswer()
    {
        if (isGame)
        {
            AddIncorrectCount();
            AddTotalCount();
        }
    }

    private void AddScore()
    {
        correctCount += 1;
    }

    private void AddTotalCount()
    {
        totalCount += 1;
    }
    private void AddIncorrectCount()
    {
        incorrectCount += 1;
    }
    public void EndGame()
    {
        isGame = false;
    }
    public float[] GetFinalResult()
    {
        float[] ansArray = { correctCount, (float)correctCount / (float)totalCount };
        return ansArray;
    }

    private void CheckIfGameFinishScore()
    {
        if (requiredScore <= correctCount)
        {
            isGame = false;
            drawExperienceController.EndGame1();

        }
    }

}
