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
    private int index = -1;
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

        numberingText.text = (numIndex + 1).ToString() + keyboardPosition.ToString();

    }
    public void SetIndexes(int xIndex, int yIndex, int zIndex)
    {
        numIndex = xIndex + gridSize * (gridSize - 1 - yIndex);
        if (zIndex == 0) keyboardPosition = KeyboardPosition.Plus;
        else if (zIndex == 1) keyboardPosition = KeyboardPosition.Minus;
        else if (zIndex == 2) keyboardPosition = KeyboardPosition.Equal;

        SetText();
    }
    public void SetIndex(int tmpIndex)
    {
        index = tmpIndex;
    }


    // public int GetNumIndex()
    // {
    //     return this.numIndex;
    // }
    public int GetIndex()
    {
        return this.index;
    }
    public void SetColor()
    {

    }
}




public enum KeyboardPosition
{
    Plus,
    Minus,
    Equal
}