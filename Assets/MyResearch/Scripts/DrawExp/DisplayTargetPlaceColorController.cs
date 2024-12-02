using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTargetPlaceColorController : MonoBehaviour
{
    [SerializeField] private Image[,,] targetPlaceDisplays; // 三次元配列に変更
    [SerializeField] private TextMeshProUGUI targetNumDisplayText;
    private int targetPlaceIndex = 0;

    private int gridSize = 3; // デフォルトのグリッドサイズ

    /// <summary>
    /// 三次元ディスプレイの設定。外部から設定可能にする。
    /// </summary>
    public void InitializeDisplays(int size, Image[,,] displays)
    {
        gridSize = size;
        targetPlaceDisplays = displays;
    }

    public void ChangeIndexAndReflect(int index)
    {
        targetPlaceIndex = index;
        ChangeIndexDisplay();
        ChangeColors();
    }

    public void ChangeIndexDisplay()
    {
        targetNumDisplayText.text = (targetPlaceIndex + 1).ToString();
    }

    public void ChangeColors()
    {
        int xIndex = targetPlaceIndex / (gridSize * gridSize);
        int yIndex = (targetPlaceIndex / gridSize) % gridSize;
        int zIndex = targetPlaceIndex % gridSize;

        // すべてのディスプレイを白にリセット
        foreach (var display in targetPlaceDisplays)
        {
            display.color = Color.white;
        }

        // ターゲットインデックスのディスプレイに色を付ける
        if (xIndex < gridSize && yIndex < gridSize && zIndex < gridSize)
        {
            Debug.Log($"Highlighting Display at Index: x={xIndex}, y={yIndex}, z={zIndex}");
            targetPlaceDisplays[xIndex, yIndex, zIndex].color = Color.red;
        }
        else
        {
            Debug.LogWarning("Target index out of bounds!");
        }
    }

    private Color DecideColor()
    {
        if (targetPlaceIndex == 0)
        {
            return Color.blue;
        }
        else if (targetPlaceIndex == 2)
        {
            return Color.red;
        }
        else if (targetPlaceIndex == 6)
        {
            return Color.yellow;
        }
        else if (targetPlaceIndex == 8)
        {
            return Color.black;
        }
        return Color.white;
    }
}
