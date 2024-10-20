using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundDesignSettingController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] CreateSoundController createSoundController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = createSoundController.GetTmpFrequency().ToString("f2");

    }
}
