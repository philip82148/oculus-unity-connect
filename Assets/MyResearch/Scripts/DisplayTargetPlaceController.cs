using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTargetPlaceController : MonoBehaviour
{

    [SerializeField] private Renderer[] targetPlaceDisplays;
    // Start is called before the first frame update
    [SerializeField] private int targetPlaceIndex = 0;


    public void ChangeColors(int targetIndex)
    {
        targetPlaceIndex = targetIndex;

        for (int i = 0; i < targetPlaceDisplays.Length; i++)
        {
            if (i == targetPlaceIndex)
            {
                targetPlaceDisplays[targetPlaceIndex].material.color = Color.blue;
            }
            else
            {
                targetPlaceDisplays[i].material.color = Color.white;
            }
        }
    }


}
