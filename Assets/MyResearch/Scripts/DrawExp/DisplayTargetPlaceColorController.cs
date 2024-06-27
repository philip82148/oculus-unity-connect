using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTargetPlaceColorController : MonoBehaviour
{
    [SerializeField] private Image[] targetPlaceDisplays;
    [SerializeField] private TextMeshProUGUI targetNumDisplayText;
    // Start is called before the first frame update
    private int targetPlaceIndex = 0;


    public void ChangeIndexAndReflect(int index)
    {
        targetPlaceIndex = index;
        ChangeIndexDisplay();
        ChangeColors();
    }


    public void ChangeIndexDisplay()
    {
        targetNumDisplayText.text = targetPlaceIndex.ToString();

    }


    public void ChangeColors()
    {




        for (int i = 0; i < targetPlaceDisplays.Length; i++)
        {
            if (i == targetPlaceIndex)
            {
                Debug.Log("called" + targetPlaceIndex);
                targetPlaceDisplays[targetPlaceIndex].color = Color.blue;
            }
            else
            {
                targetPlaceDisplays[i].color = Color.white;
            }
        }
    }
}
