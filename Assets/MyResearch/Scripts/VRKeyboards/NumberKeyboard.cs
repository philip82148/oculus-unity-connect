using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberKeyboard : MonoBehaviour
{
    [SerializeField]
    private List<KeyboardKey> keyboardKeys;
    [SerializeField] private TextMeshProUGUI problemText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private int tmpTargetIndex = -1;
    private string userInput = "";



    private string[] textStrings = {
         "Type the word: hello",
        "Type the word: world",
          "Type the word: DCC",
        "Type the word: world",
    };
    private string[] correctAnswers ={
         "Type the word: hello",
        "Type the word: world",
          "Type the word: DCC",
        "Type the word: world",
    };
    private string[] convertToAnswers = {
        "1+",
        "2+",
        "3+",
        "4+",
        "5+",
        "6+",
        "7+",
        "8+",
        "9+",
        "1-",
        "2-",
        "3-",
        "4-",
        "5-",
        "6-",
        "7-",
        "8-",
        "9-",
        "1=",
        "2=",
        "3=",
        "4=",
        "5=",
        "6=",
        "7=",
        "8=",
        "9=",

    };


    void Start()
    {
        problemText.text = textStrings[0];
    }




    void Update()
    {
        scoreText.text = "Score:" + score.ToString();

    }

    public void OnKeyPressed(string key)
    {
        userInput += key;
    }

    // public void SetNumberKeyboard(KeyboardKey keyboardKey)
    // {
    //     keyboardKeys.Add(keyboardKey);

    // }
    public void SetNextTargetText(int tmpIndex)
    {
        tmpTargetIndex = tmpIndex;
        problemText.text = textStrings[tmpTargetIndex];
    }


    public bool CheckCorrectAnswer(int ansFirstIndex, int ansSecondIndex)
    {

        string ans = convertToAnswers[ansFirstIndex] + convertToAnswers[ansSecondIndex];
        string correctAnswer = correctAnswers[tmpTargetIndex];
        Debug.Log(ans + "correct:" + correctAnswer + "text:" + textStrings[tmpTargetIndex]);
        // Debug.Log);
        if (ans.Equals(correctAnswer))
        {
            score += 1;
            return true;
        }
        else return false;
    }

    public int GetProblemCount()
    {
        return this.textStrings.Length;
    }



}
