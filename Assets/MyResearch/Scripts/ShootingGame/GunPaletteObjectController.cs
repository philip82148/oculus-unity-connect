using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPaletteObjectController : MonoBehaviour
{
    [SerializeField] private ShooterController shooterController;
    [SerializeField]
    private Color defaultColor;
    [SerializeField] private Renderer thisRenderer;
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer.material.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PanelClicked();
        }
    }

    public void PanelClicked()
    {
        shooterController.BulletCountReborn();
    }


}
