using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Meta.Voice.Samples.BuiltInTimer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrawExperienceController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField] private TimeController timeController;

    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;
    [SerializeField] private TargetSpotController targetSpotController;


    private const int PLACE_COUNT = 9;
    private int targetIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        RandomlyChoosePlace();
        ChangeDisplayColor();
        targetSpotController.SetTargetPlaceIndex(targetIndex);

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

    public void ChangeDisplayColor()
    {
        displayTargetPlaceColorController.ChangeIndexAndReflect(targetIndex);
    }

    public void RandomlyChoosePlace()
    {
        targetIndex = Random.Range(0, PLACE_COUNT);
    }
}
