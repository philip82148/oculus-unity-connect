using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.AI;

public class JoyStickMoveController : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    private Vector3 changeRotation;
    [SerializeField] private List<GameObject> cornerObjects;
    private enum Direction { North, East, South, West }
    private Direction currentDirection;
    private int floorLength = 4;
    [SerializeField] private float rotateDuration = 0.5f;
    private bool isRotating = false;
    //  [SerializeField] private float sampleDistance = 0.1f; // NavMesh上の有効位置を探す半径

    void Start()
    {
        currentDirection = Direction.North;
        AlignToCornerCenter(); // 最初に交差点の中心に位置を合わせる
    }

    void Update()
    {
        ChangeDirection();
        MoveForward();
    }

    void ChangeDirection()
    {
        if (isRotating) return;
        if (!IsAtCorner()) return;
        float angle = 90f;
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            StartCoroutine(SmoothRotate(-angle));
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            StartCoroutine(SmoothRotate(angle));
        }
    }

    private bool IsAtCorner()
    {
        foreach (GameObject corner in cornerObjects)
        {
            float withinLength = (float)floorLength / Mathf.Sqrt(2); // 許容範囲を小さく設定
            float distanceX = Mathf.Abs(this.transform.position.x - corner.transform.position.x);
            float distanceZ = Mathf.Abs(this.transform.position.z - corner.transform.position.z);
            if (distanceX < withinLength && distanceZ < withinLength)
            {
                return true;
            }
        }
        return false;
    }

    void MoveForward()
    {
        Vector2 stickInput = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        float moveValue = stickInput.y;

        if (Mathf.Abs(moveValue) < 0.8f)
            return;

        Vector3 position = transform.position;

        Vector3 moveDirection = Vector3.zero;

        switch (currentDirection)
        {
            case Direction.North:
                moveDirection = Vector3.forward;
                break;
            case Direction.South:
                moveDirection = Vector3.back;
                break;
            case Direction.East:
                moveDirection = Vector3.right;
                break;
            case Direction.West:
                moveDirection = Vector3.left;
                break;
        }

        position += moveDirection * moveValue * speed;

        transform.position = position;

        AlignToCenter(); // 移動後に位置を補正
    }

    void AlignToCenter()
    {
        Vector3 position = transform.position;

        float remainderX = position.x % floorLength;
        float remainderZ = position.z % floorLength;

        float centerX = position.x - remainderX + floorLength / 2;
        float centerZ = position.z - remainderZ + floorLength / 2;

        if (currentDirection == Direction.North || currentDirection == Direction.South)
        {
            position.x = centerX;
        }
        else if (currentDirection == Direction.East || currentDirection == Direction.West)
        {
            position.z = centerZ;
        }

        transform.position = position;
    }

    void AlignToCornerCenter()
    {
        Vector3 position = transform.position;

        float remainderX = position.x % floorLength;
        float remainderZ = position.z % floorLength;

        float centerX = position.x - remainderX + floorLength / 2;
        float centerZ = position.z - remainderZ + floorLength / 2;

        // if (currentDirection == Direction.North)
        // {
        //     position.z = position.z - remainderZ + floorLength;
        //     // position.x = position.x - remainderX + floorLength;
        // }
        // else if (currentDirection == Direction.East)
        // {
        //     position.x = position.x - remainderX + floorLength;
        // }
        // else if (currentDirection == Direction.South)
        // {

        // }
        // else if (currentDirection == Direction.West)
        // {

        // }

        // 交差点の中心に位置を合わせる
        position.x = centerX;
        position.z = centerZ;

        transform.position = position;
    }

    void UpdateDirection(float angle)
    {
        int dir = (int)currentDirection;
        dir += (int)(angle / 90);

        dir = (dir + 4) % 4;
        currentDirection = (Direction)dir;
    }

    IEnumerator SmoothRotate(float angle)
    {
        isRotating = true;

        float elapsed = 0f;
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + angle;

        // 現在位置と、回転後にAlignToCornerCenterで求まるであろう位置を計算
        Vector3 startPos = transform.position;
        Vector3 finalPos = CalculateCornerCenterPositionAfterRotation(endRotation);

        // 回転中は方向が変わるので、回転終了後にUpdateDirectionを行う
        // ここではdirectionはあらかじめ計算せず回転終了後に更新

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;

            float currentY = Mathf.Lerp(startRotation, endRotation, t);

            // 位置もLerp
            Vector3 currentPos = Vector3.Lerp(startPos, finalPos, t);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentY, transform.eulerAngles.z);
            transform.position = currentPos;

            yield return null;
        }

        // 最終的な回転角度を確定
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);
        transform.position = finalPos;

        UpdateDirection(angle);

        isRotating = false;
    }

    // 回転終了後にAlignToCornerCenter相当の処理を行った場合の座標を先に計算するメソッド
    Vector3 CalculateCornerCenterPositionAfterRotation(float endRotation)
    {
        // 一旦現在の状態を保存
        Vector3 originalPos = transform.position;
        Vector3 originalEuler = transform.eulerAngles;

        // 仮想的にendRotationへ回転・方向更新
        transform.eulerAngles = new Vector3(originalEuler.x, endRotation, originalEuler.z);
        // 仮方向を求めるため、一瞬direction更新
        float angleDiff = endRotation - originalEuler.y;
        UpdateDirection(angleDiff);

        // AlignToCornerCenterと同様の処理で最終位置を計算
        Vector3 position = transform.position;
        float remainderX = position.x % floorLength;
        float remainderZ = position.z % floorLength;

        float centerX = position.x - remainderX + floorLength / 2;
        float centerZ = position.z - remainderZ + floorLength / 2;

        if (currentDirection == Direction.North || currentDirection == Direction.South)
        {
            position.x = centerX;
        }
        else if (currentDirection == Direction.East || currentDirection == Direction.West)
        {
            position.z = centerZ;
        }

        // 元に戻しておく
        // directionはUpdateDirectionしたので元に戻す必要がある
        // 元のdirectionに戻すためには、angleDiffの逆向きのUpdateDirectionをする
        UpdateDirection(-angleDiff);
        transform.eulerAngles = originalEuler;
        transform.position = originalPos;

        return position;
    }
}
