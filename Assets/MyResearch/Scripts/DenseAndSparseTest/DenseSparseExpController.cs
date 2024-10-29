using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Body.Input;
using TMPro;
using UnityEngine;

public class DenseSparseExpController : MonoBehaviour
{

    [SerializeField] private GameObject baseObject;
    [SerializeField] private Vector3 startCoordinate;
    [SerializeField] private float interval = 0.05f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;
    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    // [SerializeField] private TextMeshProUGUI targetIndexText;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

    [SerializeField] private DenseOrSparse denseOrSparse;

    private List<Vector3> targetCoordinates = new List<Vector3>();

    private int objectCount = 5;
    private int score = 0;
    private bool isGame = false;


    private int targetCorrectIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        CreateTargetObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            isGame = true;
            SetNextTarget();
        }
        if (isGame)
        {
            scoreText.text = "score:" + score.ToString();
        }
        // if (OVRInput.GetDown(OVRInput.Button.One))
        // {
        //     SetNextTarget();
        // }
        // targetIndexText.text = "index:" + (targetCorrectIndex + 1).ToString();
    }

    public void SetNextTarget()
    {
        DecideTargetIndex();

        ChangeDisplayColor();
    }
    private void CreateTargetObjects()
    {
        int count = (int)objectCount / 2;

        GameObject gameObject = Instantiate(baseObject, startCoordinate, Quaternion.Euler(0, 0, 0));
        targetCoordinates.Add(startCoordinate);
        TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
        if (text == null)
        {
            Debug.Log("text null");
        }

        text.text = (count + 1).ToString();
        PaletteObjectController paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
        paletteObjectController.SetIndex(count);
        calculateDistance.SetTargetObject(gameObject);
        calculateDistance.SetCentralObject(gameObject);

        for (int i = 0; i < count; i++)
        {
            CreateTargetObject(i);

        }
    }
    private void CreateTargetObject(int index)
    {
        GameObject gameObject = Instantiate(baseObject, new Vector3(startCoordinate.x, startCoordinate.y + (index + 1) * interval, startCoordinate.z), Quaternion.Euler(0, 0, 0));
        targetCoordinates.Add(new Vector3(startCoordinate.x, startCoordinate.y + (index + 1) * interval, startCoordinate.z));
        TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
        if (text == null)
        {
            Debug.Log("text null");
        }
        int countDecide = (int)objectCount / 2 + 1;
        PaletteObjectController paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
        paletteObjectController.SetIndex(countDecide - index - 1 - 1);
        text.text = (countDecide - index - 1).ToString();
        calculateDistance.SetTargetObject(gameObject);



        gameObject = Instantiate(baseObject, new Vector3(startCoordinate.x, startCoordinate.y - (index + 1) * interval, startCoordinate.z), Quaternion.Euler(0, 0, 0));
        targetCoordinates.Add(new Vector3(startCoordinate.x, startCoordinate.y - (index + 1) * interval, startCoordinate.z));
        text = gameObject.GetComponentInChildren<TextMeshPro>();
        paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
        paletteObjectController.SetIndex(countDecide + index);
        text.text = (countDecide + index + 1).ToString();
        calculateDistance.SetTargetObject(gameObject);
    }



    public DenseOrSparse GetDenseOrSparse()
    {
        return denseOrSparse;

    }

    public bool GetIsGame()
    {
        return isGame;
    }


    private void DecideTargetIndex()
    {

        targetCorrectIndex = Random.Range(0, objectCount);
    }

    public void SetRejoinedIndex(int rejoinedIndex)
    {
        CheckCorrectAnswer(rejoinedIndex);
        DecideTargetIndex();
        SetNextTarget();
    }
    private void CheckCorrectAnswer(int rejoinedIndex)
    {
        if (targetCorrectIndex == rejoinedIndex)
        {
            score += 1;
        }
    }
    public void ChangeDisplayColor()
    {
        displayTargetPlaceColorController.ChangeIndexAndReflect(targetCorrectIndex);
    }


    public Vector3 GetStartCoordinate()
    {
        return startCoordinate;
    }

    public List<Vector3> GetTargetCoordinates()
    {
        return targetCoordinates;
    }

    public float GetInterval()
    {
        return interval;
    }
    public int GetObjectCount()
    {
        return objectCount;
    }

}


public enum DenseOrSparse
{
    Dense,
    Sparse
}
