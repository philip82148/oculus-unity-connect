using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseGameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore()
    {
        score += 1;
    }
}
