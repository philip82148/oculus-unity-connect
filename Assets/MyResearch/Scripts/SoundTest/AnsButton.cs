using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class AnsButton : MonoBehaviour
{
    [SerializeField]
    private ResolutionExpController resolutionExpController;
    [SerializeField] private GameObject[] frequencyButtons;
    [SerializeField] private GameObject[] amplitudeButtons;
    [SerializeField] private GameObject[] panButtons;

    private int tmpFrequencyIndex;
    private int tmpAmplitudeIndex;
    private int tmpPanIndex;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIndex(int index, ResolutionType resolutionType)
    {
        if (resolutionType == ResolutionType.Frequency)
        {
            frequencyButtons[tmpFrequencyIndex].GetComponent<Image>().color = Color.white;
            tmpFrequencyIndex = index;

            frequencyButtons[tmpFrequencyIndex].GetComponent<Image>().color = Color.green;
        }
        else if (resolutionType == ResolutionType.Amplitude)
        {
            amplitudeButtons[tmpAmplitudeIndex].GetComponent<Image>().color = Color.white;
            tmpAmplitudeIndex = index;

            amplitudeButtons[tmpAmplitudeIndex].GetComponent<Image>().color = Color.green;
        }
        else if (resolutionType == ResolutionType.Pan)
        {
            panButtons[tmpPanIndex].GetComponent<Image>().color = Color.white;
            tmpPanIndex = index;

            panButtons[tmpPanIndex].GetComponent<Image>().color = Color.green;
        }
    }
    public void OnClicked()
    {
        resolutionExpController.AnswerSetting(tmpFrequencyIndex, tmpAmplitudeIndex, tmpPanIndex);
        frequencyButtons[tmpFrequencyIndex].GetComponent<Image>().color = Color.white;
        amplitudeButtons[tmpAmplitudeIndex].GetComponent<Image>().color = Color.white;
        panButtons[tmpPanIndex].GetComponent<Image>().color = Color.white;

    }
}
