using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    [SerializeField] private const float gameTime = 60.0f;
    private float remainingTime;
    public TextMeshPro timerText;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = gameTime;
        UpdateTimerText();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void UpdateTimerText()
    {

    }
}
