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



    private string[] textStrings = {
         "Type the word: HELLO",
        "Type the word: WORLD",
          "Type the word: DCC",
        "Type the word: UIST",
         "Type the word:AUGMENTEDHUMANS",
    };
    private string[] correctAnswers ={
        "HELLO",
        "WORLD",
        "DCC",
        "UIST",
        "AUGMENTEDHUMANS"
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
        problemText.text = textStrings[tmpTargetIndex];
    }





    void Update()
    {
        scoreText.text = "Score:" + score.ToString();
        tmpText.text = userInput;
        problemText.text = textStrings[tmpTargetIndex];
        if (userInput.Length == correctAnswers[tmpTargetIndex].Length)
        {
            CheckCorrectTextAnswer();
            userInput = "";
        }

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

    private void CheckCorrectTextAnswer()
    {
        if (userInput.Equals(correctAnswers[tmpTargetIndex]))
        {
            score += 1;
        }
        SetNextTarget();
    }

    private void SetNextTarget()
    {
        tmpTargetIndex = Random.Range(0, textStrings.Length);
    }

    public int GetProblemCount()
    {
        return this.textStrings.Length;
    }



}
