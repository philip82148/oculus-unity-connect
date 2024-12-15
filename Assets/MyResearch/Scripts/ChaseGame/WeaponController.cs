using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
    [SerializeField] private float bombSpeed;
    [SerializeField] private float bulletSpeed;
    private int selectedIndex = 0;

    [Header("OVR Setting")]
    [SerializeField] private GameObject trackingSpace;
    private float bombCooldown = 1f;
    private float lastBombTime = -1f;

    private Vector3 lastPosition;
    private Vector3 playerVelocity;

    void Start()
    {
        for (int i = 1; i < weaponObjects.Count; i++)
        {
            weaponObjects[i].SetActive(false);
        }
        lastBombTime = -bombCooldown;
        lastPosition = transform.position;  // 初期位置記録
    }

    void Update()
    {
        // プレイヤー速度計算
        Vector3 currentPosition = trackingSpace.transform.position;
        playerVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            InstantiateWeapon();
        }
    }

    private void InstantiateWeapon()
    {
        Vector3 rightHandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion rightHandRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        Vector3 spawnPosition = spawnPositions[selectedIndex].transform.position;
        Quaternion spawnRotation = trackingSpace.transform.rotation * rightHandRotation;

        GameObject targetObject;
        if (selectedIndex == 0)
        {
            targetObject = bulletObject;
        }
        else if (selectedIndex == 1)
        {
            if (Time.time - lastBombTime < bombCooldown)
            {
                Debug.Log("手りゅう弾はクールダウン中です！");
                return;
            }
            else
            {
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

        GameObject weaponObject = Instantiate(targetObject, spawnPosition, Quaternion.identity);

        Vector3 direction = (targetEnemy.transform.position - spawnPosition).normalized;
        direction = spawnRotation * Vector3.forward;

        // ユーザ速度を足す
        Vector3 finalVelocity = direction * bulletSpeed + playerVelocity;

        FireWeapon(weaponObject, finalVelocity);
    }

    private void FireWeapon(GameObject weaponObject, Vector3 velocity)
    {
        if (selectedIndex == 0)
        {
            Bullet bullet = weaponObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Initialize(velocity);
            }
        }
        else if (selectedIndex == 1)
        {
            Bomb bomb = weaponObject.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.Initialize(velocity);
            }
        }
        else if (selectedIndex == 2)
        {
            Rope rope = weaponObject.GetComponent<Rope>();
            if (rope != null)
            {
                rope.Initialize(velocity);
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
