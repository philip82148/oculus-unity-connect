using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    private const int EXP_COUNT = 5;
    private const int PLACE_COUNT = 4;
    [SerializeField] private int remainingCount;
    [SerializeField] private TextMeshProUGUI counterText;
    // Start is called before the first frame update
    void Start()
    {
        remainingCount = EXP_COUNT;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SubtractCount();
        }

        counterText.text = remainingCount.ToString();

    }
    public void SubtractCount()
    {
        if (remainingCount > 0)
        {

            remainingCount -= 1;
        }
    }
}
