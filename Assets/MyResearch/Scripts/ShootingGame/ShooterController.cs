using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShooterController : MonoBehaviour
{
    // [SerializeField] private GameObject defaultBullet;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private bool isShoot = false;

    [SerializeField]
    private TextMeshProUGUI bulletCountText;

    private int bulletCount = 0;
    private const int DEFAULT_BULLET_COUNT = 5;

    // Start is called before the first frame update


    void Start()
    {
        bulletCount = DEFAULT_BULLET_COUNT;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            Shoot();
        }
        if (isShoot)
        {
            // bullet.transform.Translate(new Vector3(0, 0, 0.003f));
            bullet.transform.Translate(new Vector3(0, 0, 0.05f));
        }


        bulletCountText.text = "bullet:" + bulletCount.ToString();

    }

    private void Shoot()
    {
        if (bulletCount > 0)
        {
            isShoot = true;
            bullet = Instantiate(bullet, shootingPoint.transform.position, Quaternion.identity);
            bulletCount -= 1;
        }
    }

    public void BulletCountReborn()
    {
        bulletCount = DEFAULT_BULLET_COUNT;
    }
}
