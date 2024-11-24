using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject targetEnemy;
    [SerializeField] private GameObject bulletObject;

    [SerializeField] private Transform weaponSpawnPoint;
    [SerializeField] private float weaponSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            FireWeapon();
        }

    }

    private void FireWeapon()
    {
        // 前方にオフセットする距離
        float offsetDistance = 1.0f;

        // 弾を生成する位置を計算
        Vector3 spawnPosition = weaponSpawnPoint.position + weaponSpawnPoint.forward * offsetDistance;

        // 弾を生成
        GameObject weaponObject = Instantiate(bulletObject, spawnPosition, Quaternion.identity);

        // ターゲット方向を計算
        Vector3 direction = (targetEnemy.transform.position - spawnPosition).normalized;

        Bullet bullet = weaponObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(direction, weaponSpeed);
        }

        // Rigidbody に速度を設定
        // Rigidbody rb = weaponObject.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.velocity = direction * weaponSpeed;
        // }
    }

}
