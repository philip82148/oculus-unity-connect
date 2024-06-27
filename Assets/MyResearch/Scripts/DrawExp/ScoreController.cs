using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Oculus.Platform;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    void Initialize()
    {
        score = 0;
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }


    public void StartGame()
    {
        Initialize();
    }

    public void AddScore()
    {
        score += 1;
    }

}
