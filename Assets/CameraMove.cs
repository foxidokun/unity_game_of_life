using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Direction {
    Left,
    Right,
    Up,
    Down,
    ZoomIn,
    ZoomOut,

}

public class CameraMove : MonoBehaviour
{
    Transform camera_transform;
    Vector3 cur_pos;

    private const float MAX_XZ = 32;
    private const float MIN_Y = 20;
    private const float MAX_Y = 80;
    private const float SPEED_XZ = 20;
    private const float SPEED_Y = 50;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera_transform = camera.transform;
        cur_pos = camera.transform.localPosition;
    }

    public void Move(Direction direction, float delta_time) {
        switch (direction) {
            case Direction.Left:
                cur_pos.x -= SPEED_XZ * delta_time;
                break;
            case Direction.Right:
                cur_pos.x += SPEED_XZ * delta_time;
                break;
            case Direction.Down:
                cur_pos.z -= SPEED_XZ * delta_time;
                break;
            case Direction.Up:
                cur_pos.z += SPEED_XZ * delta_time;
                break;
            case Direction.ZoomIn:
                cur_pos.y -= SPEED_Y * delta_time;
                break;
            case Direction.ZoomOut:
                cur_pos.y += SPEED_Y * delta_time;
                break;
        }

        cur_pos.x = Math.Clamp(cur_pos.x, -MAX_XZ, +MAX_XZ);
        cur_pos.z = Math.Clamp(cur_pos.z, -MAX_XZ, +MAX_XZ);
        cur_pos.y = Math.Clamp(cur_pos.y, MIN_Y, +MAX_Y);

        camera_transform.localPosition = cur_pos;
    }
}
