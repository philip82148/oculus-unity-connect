using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;

public class JoyStickMoveController : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    private Vector3 changeRotation;
    [SerializeField] private List<GameObject> cornerObjects;
    private enum Direction { North, East, South, West }
    private Direction currentDirection;
    private int floorLength = 4;

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
        if (!IsAtCorner()) return;
        float angle = 90f;
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            this.transform.Rotate(0, -angle, 0);
            UpdateDirection(-angle);
            AlignToCornerCenter(); // 回転時に交差点の中心に位置を合わせる
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            this.transform.Rotate(0, angle, 0);
            UpdateDirection(angle);
            AlignToCornerCenter(); // 回転時に交差点の中心に位置を合わせる
        }
    }

    private bool IsAtCorner()
    {
        foreach (GameObject corner in cornerObjects)
        {
            float withinLength = (float)4 / Mathf.Sqrt(2); // 許容範囲を小さく設定
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

        if (Mathf.Abs(moveValue) < 0.5f)
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
}
