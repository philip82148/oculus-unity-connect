using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaseGameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshPro hitPointAboveText;
    [SerializeField] private TextMeshProUGUI weaponDisplayText;

    private int score = 0;
    [Header("Controller Setting")]
    [SerializeField] private HandController handController;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private DenseDataLoggerController dataLoggerController;
    [SerializeField] private TimeController timeController;
    [Header("Hand Setting")]
    [SerializeField] private GameObject targetHand;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private GameObject player;
    // private Vector3 defaultPosition;

    [Header("Subject Name")]
    [SerializeField] private string subjectName;

    private Vector3 defaultPosition = new Vector3(2f, -1.85f, 2f);

    private int weaponIndex = 0;
    private bool isGame = false;



    // Start is called before the first frame update
    void Start()
    {
        dataLoggerController.InitializeAsChaseGame();
        weaponDisplayText.text = "weapon:" + weaponIndex.ToString();
        defaultPosition = player.transform.position;
    }

    private void Initialize()
    {
        enemyController.Initialize();
        player.transform.position = defaultPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            int tmpIndex = handController.GetIndex();
            if (tmpIndex == -1) return;
            weaponIndex = tmpIndex;
            SetWeapon();
            weaponDisplayText.text = "weapon:" + weaponIndex.ToString();
            if (isGame)
            {
                dataLoggerController.WriteInformation(GetRightIndexFingerPosition());
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.Three) && !isGame)
        {
            timeController.StartGameCountDown();
            Initialize();

        }
    }
    public void CallGameStart()
    {
        isGame = true;
    }

    public void AddScore()
    {
        score += 1;
        scoreText.text = "score:" + score.ToString();
    }
    public void SetHP(int hp)
    {
        scoreText.text = "HP:" + hp.ToString();
        hitPointAboveText.text = "HP:" + hp.ToString();
    }
    private void SetWeapon()
    {
        weaponController.SetWeapon(weaponIndex);
    }

    public string GetSubjectName()
    {
        return subjectName;
    }
    private void OnDestroy()
    {
        dataLoggerController.Close();
    }
    private Vector3 GetRightIndexFingerPosition()
    {
        return targetHand.transform.position;
    }
}
