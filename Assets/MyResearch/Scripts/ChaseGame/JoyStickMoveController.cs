using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JoyStickMoveController : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private List<GameObject> cornerObjects;
    private enum Direction { North, East, South, West }
    private Direction currentDirection;
    private int floorLength = 4;

    [SerializeField] private float rotateDuration = 0.5f;
    private bool isRotating = false;

    // 従来のコーナー判定距離
    private float withinLength;
    // 手前でも曲がれるようにするための距離（前方コーナー判定用）
    private float preTurnLength = 2.5f;
    private GameObject selectedCorner;

    void Start()
    {
        currentDirection = Direction.North;
        AlignToCornerCenterInstant();
        // withinLengthを従来のロジックに合わせて設定
        withinLength = (float)floorLength / 2;
        preTurnLength = (float)floorLength / Mathf.Sqrt(2);
    }

    void Update()
    {
        ChangeDirection();
        MoveForward();
    }

    void ChangeDirection()
    {
        if (isRotating) return;
        // IsAtCornerが拡張され、withinLength以内でなくても前方コーナーが近ければtrue
        if (!IsAtCorner()) return;

        float angle = 90f;
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            StartCoroutine(SmoothRotate(-angle));
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            StartCoroutine(SmoothRotate(angle));
        }
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
        // まず、withinLength以内にコーナーがあるかどうか
        bool closeCorner = false;
        foreach (GameObject corner in cornerObjects)
        {
            float distanceX = Mathf.Abs(this.transform.position.x - corner.transform.position.x);
            float distanceZ = Mathf.Abs(this.transform.position.z - corner.transform.position.z);
            if (distanceX < withinLength && distanceZ < withinLength)
            {
                selectedCorner = corner; // このcornerで曲がる
                closeCorner = true;
                break;
            }
        }

        if (closeCorner) return true;

        // ここからはwithinLength外でも前方コーナーがpreTurnLength以内にあればtrue
        GameObject forwardCorner = GetForwardCorner();
        if (forwardCorner != null)
        {
            float distX = Mathf.Abs(this.transform.position.x - forwardCorner.transform.position.x);
            float distZ = Mathf.Abs(this.transform.position.z - forwardCorner.transform.position.z);

            // 前方コーナーまでの距離が preTurnLength以内なら、早めに曲がれるようにする
            // 前方コーナー判定は、方向に応じてXまたはZのみに注目してもよいが、
            // ここでは単純にX、Z両方を見る。
            // 必要なら( distX < preTurnLength && distZ < preTurnLength ) などに調整可能
            if (distX < preTurnLength && distZ < preTurnLength)
            {
                selectedCorner = forwardCorner; // forwardCornerで曲がるように設定
                return true;
            }
        }
        selectedCorner = null;
        return false;
    }

    // 現在向いている方向にあるコーナーオブジェクトを一つ選ぶ
    // ここでは簡易的に、現在の向き方向に最も近いコーナーを探すロジック例示
    GameObject GetForwardCorner()
    {
        Vector3 forwardDir = Vector3.zero;
        switch (currentDirection)
        {
            case Direction.North:
                forwardDir = Vector3.forward;
                break;
            case Direction.East:
                forwardDir = Vector3.right;
                break;
            case Direction.South:
                forwardDir = Vector3.back;
                break;
            case Direction.West:
                forwardDir = Vector3.left;
                break;
        }

        GameObject closestCorner = null;
        float closestDist = Mathf.Infinity;

        // 現在位置
        Vector3 pos = transform.position;

        foreach (GameObject corner in cornerObjects)
        {
            Vector3 dirToCorner = (new Vector3(corner.transform.position.x, 0, corner.transform.position.z) - new Vector3(pos.x, 0, pos.z)).normalized;

            // forwardDirとの内積で、前方方向かどうかを判定
            float dot = Vector3.Dot(forwardDir, dirToCorner);
            // dotが0.7以上くらいで、前方方向とみなすなど、閾値を設けても良い
            // ここでは前方180度内にある全コーナーを候補にするため dot>0でOK
            if (dot > 0.9)
            {
                float dist = Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(corner.transform.position.x, 0, corner.transform.position.z));
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestCorner = corner;
                }
            }
        }

        return closestCorner;
    }

    void MoveForward()
    {
        Vector2 stickInput = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        float moveValue = stickInput.y;

        // if (Mathf.Abs(moveValue) < 0.8f)
        //     return;

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

        AlignToCenterInstant();
    }

    void AlignToCenterInstant()
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

    void AlignToCornerCenterInstant()
    {
        Vector3 position = transform.position;

        float remainderX = position.x % floorLength;
        float remainderZ = position.z % floorLength;

        float centerX = position.x - remainderX + floorLength / 2;
        float centerZ = position.z - remainderZ + floorLength / 2;

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

        Vector3 finalPos = CalculateCornerCenterPositionAfterRotation(endRotation);
        Vector3 startPos = transform.position;

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;

            float currentY = Mathf.Lerp(startRotation, endRotation, t);
            Vector3 currentPos = Vector3.Lerp(startPos, finalPos, t);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentY, transform.eulerAngles.z);
            transform.position = currentPos;

            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);
        transform.position = finalPos;

        UpdateDirection(angle);

        isRotating = false;
        // 回転終了後、必要ならselectedCornerをnullに戻す
        selectedCorner = null;
    }

    Vector3 CalculateCornerCenterPositionAfterRotation(float endRotation)
    {
        Vector3 originalPos = transform.position;
        Vector3 originalEuler = transform.eulerAngles;

        transform.eulerAngles = new Vector3(originalEuler.x, endRotation, originalEuler.z);
        float angleDiff = endRotation - originalEuler.y;
        UpdateDirection(angleDiff);

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

        // selectedCornerがある場合は、selectedCornerに基づいて位置補正
        if (selectedCorner != null)
        {
            // 例: selectedCornerの位置に合わせてさらに補正
            // ここでは単純にselectedCornerの位置を基準に若干オフセットする例を示す
            // 必要に応じて、selectedCornerを中心としてfloorLength調整等を行う
            Vector3 cornerPos = selectedCorner.transform.position;

            // たとえば、selectedCornerを中心に同様のAlign処理を行うなど：
            float remainderX_c = cornerPos.x % floorLength;
            float remainderZ_c = cornerPos.z % floorLength;

            float centerX_c = cornerPos.x - remainderX_c + floorLength / 2;
            float centerZ_c = cornerPos.z - remainderZ_c + floorLength / 2;

            // selectedCornerを優先してpositionを再計算
            // 必要なら、positionをcornerに近づけるなどのロジックを追加
            // ここでは例として、selectedCornerを基準にした位置に更新する
            if (currentDirection == Direction.North || currentDirection == Direction.South)
            {
                position.x = centerX_c;
            }
            else if (currentDirection == Direction.East || currentDirection == Direction.West)
            {
                position.z = centerZ_c;
            }

            // さらに、selectedCornerが存在する場合、コーナーに合わせて余計なoffsetを除くなどの微調整が可能
            // ここは要件に合わせて適宜変更してください
        }

        UpdateDirection(-angleDiff);
        transform.eulerAngles = originalEuler;
        transform.position = originalPos;

        return position;
    }
}
