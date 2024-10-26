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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;


    [SerializeField] private DenseOrSparse denseOrSparse;

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
        if (isGame)
        {
            scoreText.text = score.ToString();
        }
    }


    private void CreateTargetObjects()
    {
        int count = (int)objectCount / 2;

        GameObject gameObject = Instantiate(baseObject, startCoordinate, Quaternion.Euler(0, 0, 0));
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        text.text = (count + 1).ToString();
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
        TextMeshProUGUI text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        text.text = (index + 1).ToString();
        calculateDistance.SetTargetObject(gameObject);
        gameObject = Instantiate(baseObject, new Vector3(startCoordinate.x, startCoordinate.y - (index + 1) * interval, startCoordinate.z), Quaternion.Euler(0, 0, 0));
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        int countDecide = (int)objectCount / 2 + 1;
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
    }
    private void CheckCorrectAnswer(int rejoinedIndex)
    {
        if (targetCorrectIndex == rejoinedIndex)
        {
            score += 1;
        }
    }

}


public enum DenseOrSparse
{
    Dense,
    Sparse
}
