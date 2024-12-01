using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject targetEnemy;
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

    // Start is called before the first frame update
    void Start()
    {

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

        Vector3 spawnPosition = trackingSpace.transform.TransformPoint(rightHandPosition);
        Quaternion spawnRotation = trackingSpace.transform.rotation * rightHandRotation;
        // 前方にオフセットする距離
        float offsetDistance = 0.05f;

        // 弾を生成する位置を計算
        // Vector3 spawnPosition = weaponSpawnPoint.position + weaponSpawnPoint.forward * offsetDistance;

        GameObject targetObject;
        if (selectedIndex == 0) targetObject = bulletObject;
        else if (selectedIndex == 1) targetObject = bombObject;
        else if (selectedIndex == 2) targetObject = ropeObject;
        else return;


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


    }


    public void SetWeapon(int index)
    {
        selectedIndex = index;
    }

}
