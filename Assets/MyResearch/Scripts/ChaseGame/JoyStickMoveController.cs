using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;

public class JoyStickMoveController : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] Vector3 changeRotation;
    [SerializeField] private List<GameObject> cornerObjects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {
        // Move3();
        ChangeDirection();
        Move4();
    }


    // private void Move()
    // {
    //     Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
    //     Vector3 changePosition = new Vector3(stickR.x, 0, stickR.y);
    //     //HMDのY軸の角度取得
    //     changeRotation = new Vector3(0, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.y, 0);
    //     //OVRCameraRigの位置変更
    //     this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition) * speed;

    // }
    private void Move2()
    {
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Vector3 changePosition = new Vector3(stickR.x, 0, stickR.y);

        // HMDのY軸の角度取得（新しいAPIを使用）
        InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        Quaternion headRotation;
        if (headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out headRotation))
        {
            changeRotation = new Vector3(0, headRotation.eulerAngles.y, 0);
        }
        else
        {
            // 取得に失敗した場合の処理
            changeRotation = Vector3.zero;
        }

        // OVRCameraRigの位置変更
        this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition) * speed;
    }
    private void Move3()
    {
        // 左スティックの入力を取得
        Vector2 stickInput = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        Vector3 direction = new Vector3(stickInput.x, 0, stickInput.y);

        // 入力がない場合は何もしない
        if (direction.magnitude < 0.1f)
            return;

        // プレイヤーの体の向きを取得
        float yRotation = transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, yRotation, 0);

        // 移動
        Vector3 moveDirection = rotation * direction.normalized * speed;
        transform.position += moveDirection;
    } //OVRCameraRigの角度変更
    void ChangeDirection()
    {

        float angle = 90f;
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            this.transform.Rotate(0, -angle, 0);
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            this.transform.Rotate(0, angle, 0);
        }
    }

    private bool IsAtCorner()
    {
        foreach (GameObject corner in cornerObjects)
        {
            float distance = Mathf.Abs()
            if (distance < 0.5f) // 許容範囲を0.5メートルとする
            {
                return true;
            }
        }
        return false;

    }

    void Move4()
    {
        //右ジョイスティックの情報取得
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        // プレイヤーの体の向きを取得
        float yRotation = transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
        //OVRCameraRigの位置変更
        this.transform.position += rotation * (new Vector3((stickR.x), 0, (stickR.y)).normalized) * speed;
    }
}
