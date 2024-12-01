using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    private float gravity = -0.981f / 3; // 重力加速度
    private float verticalSpeed = 0f; // 垂直方向の速度

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(Vector3 direction, float speed)
    {
        this.moveDirection = direction;
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        // 垂直方向の速度に重力を適用
        verticalSpeed += gravity * Time.deltaTime;

        // 現在の移動方向に垂直速度を加算
        Vector3 currentDirection = moveDirection + new Vector3(0, verticalSpeed, 0);

        // 毎フレーム移動する
        transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);
    }
}
