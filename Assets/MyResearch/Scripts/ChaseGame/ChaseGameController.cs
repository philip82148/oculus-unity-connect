using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseGameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    [SerializeField] private HandController handController;



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
        scoreText.text = "score:" + score.ToString();
    }
    public void SetHP(int hp)
    {
        scoreText.text = "HP:" + hp.ToString();
    }
}
