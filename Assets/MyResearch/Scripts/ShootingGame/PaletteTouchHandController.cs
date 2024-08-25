using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaletteTouchHandController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private ShooterController shooterController;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerStay(Collider otherObject)
    {
        GunPaletteObjectController gunPaletteObjectController = otherObject.GetComponent<GunPaletteObjectController>();
        if (gunPaletteObjectController == null) return;
        debugText.text = "success";

        if (OVRInput.Get(OVRInput.Button.One))
        {
            shooterController.BulletCountReborn();
        }
    }
}
