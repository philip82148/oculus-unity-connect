using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseGameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI weaponDisplayText;

    private int score = 0;
    [Header("Controller Setting")]
    [SerializeField] private HandController handController;
    [SerializeField] private WeaponController weaponController;
    private int weaponIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        weaponDisplayText.text = "score:" + weaponIndex.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            int tmpIndex = handController.GetIndex();
            if (tmpIndex == -1) return;
            weaponIndex = tmpIndex;
            SetWeapon();
            weaponDisplayText.text = "score:" + weaponIndex.ToString();
        }
    }

    public void AddScore()
    {
        score += 1;
        scoreText.text = "score:" + score.ToString();
    }
    public void SetHP(int hp)
    {
        scoreText.text = "HP:" + hp.ToString();
    }
    private void SetWeapon()
    {
        weaponController.SetWeapon(weaponIndex);
    }
}
