using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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

    [Header("Alphabet")]
    [SerializeField] private List<TextMeshProUGUI> keyboardDisplays = new List<TextMeshProUGUI>();

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
        int index = correctAnswers[tmpTargetIndex][0] - 'A';
        SetTextColor();
        Debug.Log("indexindex" + index);
    }





    void Update()
    {
        scoreText.text = "Score:" + score.ToString();
        tmpText.text = userInput;
        problemText.text = "Type the word:" + correctAnswers[tmpTargetIndex];
        if (correctAnswers[tmpTargetIndex].Length <= userInput.Length)
        {
            CheckCorrectTextAnswer();

        }

    }
    public char TmpTargetKey()
    {
        return correctAnswers[tmpTargetIndex][userInput.Length];
    }

    public void OnKeyPressed(string key)
    {
        if (!key.Equals("ERASE"))
        {
            if (correctAnswers[tmpTargetIndex][userInput.Length] == key[0])
            {
                score += 1;
            }
            userInput += key;

            // int nextIndex = correctAnswers[tmpTargetIndex][userInput.Length - 1];
            if (userInput.Length > 0 && key.Length > 0 && key[0] == correctAnswers[tmpTargetIndex][userInput.Length - 1])
            {
                SetTextColor();
            }
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
        SetTextColor();
        // int index = correctAnswers[tmpTargetIndex][0] - 'A';
        // keyboardDisplays[index].color = Color.red;
    }

    private void SetTextColor()
    {
        if (userInput.Length == 0)
        {
            int index = correctAnswers[tmpTargetIndex][0] - 'A';
            keyboardDisplays[index].color = Color.red;
        }
        else if (0 < userInput.Length && userInput.Length < correctAnswers[tmpTargetIndex].Length)
        {
            int tmpIndex = correctAnswers[tmpTargetIndex][userInput.Length - 1] - 'A';
            keyboardDisplays[tmpIndex].color = Color.black;
            int index = correctAnswers[tmpTargetIndex][userInput.Length] - 'A';
            keyboardDisplays[index].color = Color.red;
        }
        else
        {
            // 全て入力済みの場合、ハイライトを解除（全てblackに戻すなど）
            // もしくは必要な処理なしでもよい。
            // ここで最終文字をblackに戻したいなら:
            int prevIndex = correctAnswers[tmpTargetIndex][correctAnswers[tmpTargetIndex].Length - 1] - 'A';
            keyboardDisplays[prevIndex].color = Color.black;
        }

    }


    // public bool CheckCorrectAnswer(int ansFirstIndex, int ansSecondIndex)
    // {

    //     string ans = convertToAnswers[ansFirstIndex] + convertToAnswers[ansSecondIndex];
    //     string correctAnswer = correctAnswers[tmpTargetIndex];
    //     Debug.Log(ans + "correct:" + correctAnswer + "text:" + "Type the word:" + correctAnswers[tmpTargetIndex]);
    //     // Debug.Log);
    //     if (ans.Equals(correctAnswer))
    //     {
    //         score += 1;
    //         return true;
    //     }
    //     else return false;
    // }

    private void CheckCorrectTextAnswer()
    {
        if (userInput.Equals(correctAnswers[tmpTargetIndex]))
        {
            // score += 1;


        }
        if (userInput.Length > 0)
        {
            int tmpIndex = correctAnswers[tmpTargetIndex][userInput.Length - 1] - 'A';
            keyboardDisplays[tmpIndex].color = Color.black;
        }

        SetNextTarget(); // 次のターゲットへ
        userInput = "";
    }
    public void ResetScore()
    {
        score = 0;
        tmpTargetIndex = 0;
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
        userInput = "";
        SetTextColor();
    }


    public int GetProblemCount()
    {
        return this.correctAnswers.Length;
    }



}
