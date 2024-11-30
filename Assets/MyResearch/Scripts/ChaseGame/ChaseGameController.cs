using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseGameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    [Header("Controller Setting")]
    [SerializeField] private HandController handController;
    [SerializeField] private WeaponController weaponController;
    private int tmpIndex = 0;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            tmpIndex = handController.GetIndex();
            SetWeapon();
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
        weaponController.SetWeapon(tmpIndex);
    }
}
