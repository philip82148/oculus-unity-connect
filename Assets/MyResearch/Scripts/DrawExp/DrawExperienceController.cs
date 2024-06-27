using System.Collections;
using System.Collections.Generic;
using Meta.Voice.Samples.BuiltInTimer;
using UnityEngine;

public class DrawExperienceController : MonoBehaviour
{
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField] private TimeController timeController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            timeController.StartGameCountDown();
            scoreController.StartGame();
        }
    }

    public void AddScore()
    {
        scoreController.AddScore();
    }
}
