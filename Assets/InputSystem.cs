using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    TilePlacer field_object;
    GameObject help_menu;
    CameraMove camera_ctl;
    private bool help_opened = true;

    private bool was_running_when_opened = false;

    void Start() {
        field_object = GetComponent<TilePlacer>();
        help_menu = GameObject.Find("Help Canvas");
        camera_ctl = GameObject.Find("Main Camera").GetComponent<CameraMove>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            help_opened = !help_opened;

            if (help_opened) {
                was_running_when_opened = field_object.running;
                field_object.running = false;
                help_menu.SetActive(true);
            } else {
                field_object.running = was_running_when_opened;
                help_menu.SetActive(false);
            }
        }

        /* Ignore other input if menu is opened */
        if (help_opened) {
            return;
        }

        /* Pause */
        if (Input.GetKeyDown(KeyCode.P))
        {
            field_object.running = !field_object.running;
        }

        /* Camera movements */
        if (Input.GetKey(KeyCode.W)) {
            camera_ctl.Move(Direction.Up, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            camera_ctl.Move(Direction.Left, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            camera_ctl.Move(Direction.Down, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            camera_ctl.Move(Direction.Right, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q)) {
            camera_ctl.Move(Direction.ZoomOut, Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            camera_ctl.Move(Direction.ZoomIn, Time.deltaTime);
        }
    }
}