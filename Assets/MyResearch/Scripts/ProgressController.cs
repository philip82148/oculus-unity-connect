using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressController : MonoBehaviour
{

    [Header("Controller Setting")]
    [SerializeField] private ExperienceController experienceController;

    private const int EXP_COUNT = 5;
    private const int PLACE_COUNT = 4;

    public int ExpProgress = 0;
    [SerializeField] private int remainingCount;
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private TextMeshProUGUI counterText;


    public int[] expOrderArray;
    // Start is called before the first frame update
    void Start()
    {
        remainingCount = EXP_COUNT;
        counterText.text = remainingCount.ToString();

        expOrderArray = GenerateRandomArray();
        placeText.text = expOrderArray[ExpProgress].ToString();
        experienceController.SetTargetPlaceIndex(expOrderArray[ExpProgress]);
    }


    public void SubtractCount()
    {
        if (remainingCount > 0)
        {

            remainingCount -= 1;
            counterText.text = remainingCount.ToString();

            if (remainingCount == 0)
            {
                SwitchExpPlace();
            }

        }
    }

    private int[] GenerateRandomArray()
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < PLACE_COUNT; i++)
        {
            numbers.Add(i);
        }
        int[] randomArray = new int[4];

        for (int i = 0; i < randomArray.Length; i++)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            randomArray[i] = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);
        }

        return randomArray;

    }
    private void SwitchExpPlace()
    {
        ExpProgress += 1;

        if (PLACE_COUNT <= ExpProgress)
        {
            NoticeTestFinish();


        }
        else
        {
            experienceController.SetTargetPlaceIndex(expOrderArray[ExpProgress]);
            placeText.text = expOrderArray[ExpProgress].ToString();
            remainingCount = EXP_COUNT;
            counterText.text = remainingCount.ToString();

        }

    }

    private void NoticeTestFinish()
    {

        counterText.text = "";
        placeText.text = "Test Finished";
    }

}
