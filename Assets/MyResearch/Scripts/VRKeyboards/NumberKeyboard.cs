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
    [SerializeField] private TextMeshProUGUI tmpText;

    [SerializeField] private VRKeyboardExpController vRKeyboardExpController;

    private int score = 0;
    private int tmpTargetIndex = 0;
    private string userInput = "";
    private int objectCount = 27;



    // private string[] textStrings = {
    //      "Type the word: HELLO",
    //     "Type the word: WORLD",
    //       "Type the word: DCC",
    //     "Type the word: UIST",
    //      "Type the word:A",
    //      "Type the word:H",
    // };
    private string[] correctAnswers = {
    "A",
    "H",

    "HI",
    "BY",
    "DO",

    "CAT",
    "DOG",
    "SUN",

    "LIFE",
    "LOVE",
    "UIST",

    "HELLO",
    "WORLD",
    "DCC"
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
        objectCount = vRKeyboardExpController.GetObjectCount();
        problemText.text = "Type the word:" + correctAnswers[tmpTargetIndex];
    }





    void Update()
    {
        scoreText.text = "Score:" + score.ToString();
        tmpText.text = userInput;
        problemText.text = "Type the word:" + correctAnswers[tmpTargetIndex];
        if (correctAnswers[tmpTargetIndex].Length <= userInput.Length)
        {
            CheckCorrectTextAnswer();
            userInput = "";
        }

    }

    public void OnKeyPressed(string key)
    {
        if (!key.Equals("ERASE"))
        {
            userInput += key;

        }
        else
        {
            userInput = userInput.Remove(userInput.Length - 1);
        }
    }

    public void OnDeleteKeyPressed()
    {
        userInput = userInput.Remove(userInput.Length - 1);

    }

    // public void SetNumberKeyboard(KeyboardKey keyboardKey)
    // {
    //     keyboardKeys.Add(keyboardKey);

    // }
    public void SetNextTargetText(int tmpIndex)
    {
        tmpTargetIndex = tmpIndex;
        problemText.text = "Type the word:" + correctAnswers[tmpTargetIndex];
    }


    public bool CheckCorrectAnswer(int ansFirstIndex, int ansSecondIndex)
    {

        string ans = convertToAnswers[ansFirstIndex] + convertToAnswers[ansSecondIndex];
        string correctAnswer = correctAnswers[tmpTargetIndex];
        Debug.Log(ans + "correct:" + correctAnswer + "text:" + "Type the word:" + correctAnswers[tmpTargetIndex]);
        // Debug.Log);
        if (ans.Equals(correctAnswer))
        {
            score += 1;
            return true;
        }
        else return false;
    }

    private void CheckCorrectTextAnswer()
    {
        if (userInput.Equals(correctAnswers[tmpTargetIndex]))
        {
            score += 1;
        }
        SetNextTarget();
    }
    public void ResetScore()
    {
        score = 0;
    }
    // private void SetNextTarget()
    // {
    //     tmpTargetIndex = Random.Range(0, textStrings.Length);
    // }
    private void SetNextTarget()
    {
        // インデックスを1増やし、全問題を出題し終えたら先頭に戻る
        tmpTargetIndex = (tmpTargetIndex + 1) % correctAnswers.Length;
        problemText.text = "Type the word:" + correctAnswers[tmpTargetIndex];
    }


    public int GetProblemCount()
    {
        return this.correctAnswers.Length;
    }



}
