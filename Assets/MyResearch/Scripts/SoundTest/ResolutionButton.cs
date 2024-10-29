using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResolutionButton : MonoBehaviour
{
    [SerializeField] private int resolutionIndex = 0;
    [SerializeField]
    private ResolutionType resolutionType;
    [SerializeField] private ResolutionExpController resolutionExpController;
    [SerializeField] private AnsButton ansButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClicked()
    {
        resolutionExpController.ReflectClickedIndex(resolutionIndex, resolutionType);
        ansButton.SetIndex(resolutionIndex, resolutionType);
        // ChangeColor();
    }

    // public void ChangeColor()
    // {
    //     this.gameObject.GetComponent<Renderer>().material.color = Color.red;
    // }
}
