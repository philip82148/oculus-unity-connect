using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    private float primaryFrequency = 622.254f;
    private float secondaryFrequency = 440.0f;

    [SerializeField] private float frequency;

    public CreateSoundController createSoundController;

    // Update is called once per frame
    void Update()
    {
        frequency = (float)createSoundController.frequency;

        if (MathF.Abs(primaryFrequency - frequency) < 50f)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.blue;
        }

    }
}
