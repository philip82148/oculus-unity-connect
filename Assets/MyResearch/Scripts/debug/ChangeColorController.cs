using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private int placeIndex;
    [SerializeField] private Renderer[] rendererList;
    private float primaryFrequency = 622.254f;
    // private float secondaryFrequency = 440.0f;
    private float frequency;

    [SerializeField] private CreateSoundController createSoundController;
    private int tmpPlaceIndex = 0;



    public void SetTargetPlaceIndex(int index)
    {
        rendererList[tmpPlaceIndex].material.color = Color.blue;
        tmpPlaceIndex = index;
    }

    void Update()
    {
        frequency = (float)createSoundController.frequency;

        if (MathF.Abs(primaryFrequency - frequency) < 50f)
        {
            rendererList[tmpPlaceIndex].material.color = Color.red;
        }
        else
        {
            rendererList[tmpPlaceIndex].material.color = Color.blue;
        }

    }
}
