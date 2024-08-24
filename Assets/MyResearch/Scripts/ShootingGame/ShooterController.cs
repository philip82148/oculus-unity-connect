using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    // [SerializeField] private GameObject defaultBullet;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private bool isShoot = false;

    // Start is called before the first frame update


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (isShoot)
        {
            bullet.transform.Translate(new Vector3(0, 0, 0.003f));
        }

    }

    private void Shoot()
    {
        isShoot = true;
        bullet = Instantiate(bullet, shootingPoint.transform.position, Quaternion.identity);
    }
}
