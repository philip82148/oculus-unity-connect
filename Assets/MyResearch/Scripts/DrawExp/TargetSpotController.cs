using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpotController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private DrawExperienceController drawExperienceController;

    [SerializeField] private Image thisImage;
    private bool isCollision = false;
    private Color defaultColor;
    private int targetPlaceIndex;

    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = Color.white;

    }


    private void SetColor(Color color)
    {
        thisImage.color = color;
    }

    public void SetTargetPlaceIndex(int index)
    {
        targetPlaceIndex = index;
    }


    void OnTriggerEnter(Collider other)
    {
        collidingObjects.Add(other.gameObject);
        PaletteObjectController paletteObjectController = other.GetComponent<PaletteObjectController>();
        if (paletteObjectController != null)
        {
            int objectIndex = paletteObjectController.GetIndex();
            if (objectIndex == targetPlaceIndex)
            {
                SetColor(Color.green);
                GetCorrectAnswer();
            }
            else
            {
                SetColor(Color.red);
                GetIncorrectAnswer();

            }

        }
    }

    public void GetCorrectAnswer()
    {
        drawExperienceController.GetCorrectAnswer();
    }
    public void GetIncorrectAnswer()
    {
        drawExperienceController.GetIncorrectAnswer();
    }

    void OnTriggerExit(Collider other)
    {
        SetColor(defaultColor);
        collidingObjects.Remove(other.gameObject);
    }
}
