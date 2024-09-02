using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AngularSoundController : MonoBehaviour
{
    [SerializeField] GameObject playerKatana;
    [SerializeField] CreateSoundController createSoundController;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        createSoundController.SetAmplitude(1);
        createSoundController.SetPan(0);
        float roY = playerKatana.transform.rotation.y;
        roY = playerKatana.transform.eulerAngles.y;

        if (0 < roY && roY < 30)
        {
            createSoundController.frequencyCoefficient = 0.5;
        }
        else if (30 <= roY && roY <= 45)
        {
            createSoundController.frequencyCoefficient = 1;
        }
        else if (45 < roY && roY < 90)
        {
            createSoundController.frequencyCoefficient = 2;
        }
        text.text = roY.ToString();
    }
}
