using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberKeyboard : MonoBehaviour
{
    [SerializeField]
    private KeyboardKey[] keyboardKeys;

    private string[] textStrings = {
        "バナナ2本とオレンジ3個を足す式を書いてください。",
        "ぶどう4房とりんご1個を足す式を書いてください。",
        "梨3個と桃2個を足す式を書いてください。",
        "りんご5個からバナナ2本を引く式を書いてください。",
        "みかん6個からレモン1個を引く式を書いてください。"
    };
    private string[] correctAnswers = {
        "2+3=",
        "4+1=",
        "3+2=",
        "5-2=",
        "6-1="
    };


    void Start()
    {

    }




    void Update()
    {

    }

    public void SetNumberKeyboard()
    {

    }
}
