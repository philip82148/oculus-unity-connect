using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotTargetController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "score:" + score.ToString();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("shot");
        // targetRenderer.material.color = Color.red;
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("shot");
        // targetRenderer.material.color = Color.red;
    }

    public void CallShot()
    {
        score += 1;
    }
}
