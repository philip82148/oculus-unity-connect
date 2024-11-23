using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardKey : MonoBehaviour
{
    [Header("UI Setting")]
    [SerializeField]
    private TextMeshPro numberingText;
    private int numIndex = 0;
    private KeyboardPosition keyboardPosition;
    private int gridSize = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SetText()
    {
        // if (expScene == ExpScene.DenseOrSparse)
        // {
        //     numberingText.text = (index + 1).ToString();
        // }

        numberingText.text = (numIndex).ToString() + keyboardPosition.ToString();

    }
    public void SetIndexes(int xIndex, int yIndex, int zIndex)
    {
        numIndex = xIndex + gridSize * yIndex;
        if (zIndex == 0) keyboardPosition = KeyboardPosition.Plus;
        else if (zIndex == 1) keyboardPosition = KeyboardPosition.Minus;
        else if (zIndex == 2) keyboardPosition = KeyboardPosition.Equal;

        SetText();
    }

    public int GetNumIndex()
    {
        return this.numIndex;
    }
}




public enum KeyboardPosition
{
    Plus,
    Minus,
    Equal
}