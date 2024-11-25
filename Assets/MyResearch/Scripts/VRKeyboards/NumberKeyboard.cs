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



    private string[] textStrings = {
        "バナナ2本とオレンジ3個を足す式を書いてください。",
        "ぶどう4房とりんご1個を足す式を書いてください。",
        "梨3個と桃2個を足す式を書いてください。",
        "りんご5個からバナナ2本を引く式を書いてください。",
        "みかん6個からレモン1個を引く式を書いてください。"
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

    public void SetNumberKeyboard(KeyboardKey keyboardKey)
    {
        keyboardKeys.Add(keyboardKey);

    }
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
