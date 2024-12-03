using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject targetEnemy;
    [Header("Weapon")]
    [SerializeField] private List<GameObject> weaponObjects;
    [SerializeField] private List<GameObject> spawnPositions;
    [Header("Bullet Object")]

    [SerializeField] private GameObject bulletObject;
    [SerializeField] private GameObject bombObject;
    [SerializeField] private GameObject ropeObject;

    [SerializeField] private Transform weaponSpawnPoint;
    // [SerializeField] private float weaponSpeed;
    [SerializeField] private float bombSpeed;
    [SerializeField] private float bulletSpeed;
    private int selectedIndex = 0;

    [Header("OVR Setting")]
    [SerializeField] private GameObject trackingSpace;
    private float bombCooldown = 5f; // クールダウン時間（秒）
    private float lastBombTime = -5f; // 最後に手りゅう弾を発射した時間

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < weaponObjects.Count; i++)
        {
            weaponObjects[i].SetActive(false);
        }
        lastBombTime = -bombCooldown; // ゲーム開始時にすぐ発射できるように初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            InstantiateWeapon();
        }

    }

    private void InstantiateWeapon()
    {
        Vector3 rightHandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion rightHandRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        // Vector3 spawnPosition = trackingSpace.transform.TransformPoint(rightHandPosition);
        Vector3 spawnPosition = spawnPositions[selectedIndex].transform.position;
        Quaternion spawnRotation = trackingSpace.transform.rotation * rightHandRotation;
        // 前方にオフセットする距離
        float offsetDistance = 0.05f;

        // 弾を生成する位置を計算
        // Vector3 spawnPosition = weaponSpawnPoint.position + weaponSpawnPoint.forward * offsetDistance;

        GameObject targetObject;
        if (selectedIndex == 0)
        {
            targetObject = bulletObject;
            // weaponObjects[se]
        }
        else if (selectedIndex == 1)
        {
            // 手りゅう弾のクールダウンをチェック
            if (Time.time - lastBombTime < bombCooldown)
            {
                // クールダウン中は発射できない
                Debug.Log("手りゅう弾はクールダウン中です！");
                return;
            }
            else
            {
                // 発射可能なので、最後に発射した時間を更新
                lastBombTime = Time.time;
                targetObject = bombObject;
            }
        }
        else if (selectedIndex == 2)
        {
            targetObject = ropeObject;
        }
        else
        {
            return;
        }

        // 弾を生成
        GameObject weaponObject = Instantiate(targetObject, spawnPosition, Quaternion.identity);

        // ターゲット方向を計算
        Vector3 direction = (targetEnemy.transform.position - spawnPosition).normalized;

        direction = spawnRotation * Vector3.forward;
        FireWeapon(weaponObject, direction);

        // Rigidbody に速度を設定
        // Rigidbody rb = weaponObject.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.velocity = direction * weaponSpeed;
        // }

    }


    private void FireWeapon(GameObject weaponObject, Vector3 direction)
    {
        if (selectedIndex == 0)
        {
            Bullet bullet = weaponObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Initialize(direction, bulletSpeed);
            }
        }
        else if (selectedIndex == 1)
        {
            Bomb bomb = weaponObject.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.Initialize(direction, bombSpeed);
            }
        }
        else if (selectedIndex == 2)
        {
            Rope rope = weaponObject.GetComponent<Rope>();
            if (rope != null)
            {
                rope.Initialize(direction, bombSpeed);
            }
        }


    }


    public void SetWeapon(int index)
    {
        weaponObjects[selectedIndex].SetActive(false);
        selectedIndex = index;
        weaponObjects[selectedIndex].SetActive(true);
    }

}
