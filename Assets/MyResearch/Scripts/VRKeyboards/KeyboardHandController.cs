using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandController : MonoBehaviour
{
    [SerializeField]
    private VRKeyboardExpController vRKeyboardExpController;
    [SerializeField] private NumberKeyboard numberKeyboard;

    private bool isTouched = false;
    private string tmpAlphabet;

    // 今のフレームで最も近かったKeyboardKey
    private KeyboardKey closestKey = null;
    private GameObject closestObject = null;
    private float closestDistance = Mathf.Infinity;

    void Update()
    {
        // フレームごとにclosestKeyをリセット
        // 毎フレームOnTriggerStayが呼ばれるので、その中で判定


        // OVRInputのチェックはUpdateで行う
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            // ボタンが押されたタイミングで、前フレームまでの衝突情報を使い
            // 最も近いキーが決まっている場合は入力を行う
            if (closestKey != null)
            {
                closestKey.SetColor();
                numberKeyboard.OnKeyPressed(closestKey.GetAlphabet());
            }
        }
    }

    private void OnTriggerStay(Collider otherObject)
    {
        KeyboardKey keyboardKey = otherObject.GetComponent<KeyboardKey>();
        if (keyboardKey == null)
        {
            return;
        }

        isTouched = true;

        // このキーまでの距離を計算して、最も近いものを記録
        float dist = Vector3.Distance(transform.position, otherObject.transform.position);
        if (dist < closestDistance)
        {
            closestDistance = dist;
            closestObject = otherObject.gameObject;
            closestKey = keyboardKey;
            tmpAlphabet = keyboardKey.GetAlphabet();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // オブジェクトが離れた場合、もう一度ClosestKeyを求め直す必要がある
        // ただしOnTriggerStayが毎フレーム呼ばれるため、次フレームで再計算される
        // ここでは特に何もせず、Updateで再判定させる
        // オブジェクトが減ってclosestKeyがなくなる場合は、Updateの最初でリセットされる
        if (other.gameObject == closestObject)
        {
            closestObject = null;
            closestDistance = Mathf.Infinity;
        }
    }

    public string GetAlphabet()
    {
        if (isTouched)
        {
            return tmpAlphabet;
        }
        else return "";
    }
}
