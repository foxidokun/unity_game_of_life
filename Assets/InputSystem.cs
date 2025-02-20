using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class InputSystem : MonoBehaviour
{
    /* References to other components */
    private FieldLogic field_logic;
    private GameObject help_menu;
    private CameraMove camera_ctl;
    private StatusBar statusbar_ctl;
    private UserLogic user_ctl;
    private bool help_opened = true;

    private bool was_running_when_opened = false;

    void Start() {
        field_logic = GetComponent<FieldLogic>();
        help_menu = GameObject.Find("Help Canvas");
        camera_ctl = FindObjectOfType<CameraMove>();
        statusbar_ctl = FindObjectOfType<StatusBar>();
        user_ctl = FindObjectOfType<UserLogic>();
    }

    void Update() {
        if (field_logic.finished) {
            HandleCameraMoves();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            help_opened = !help_opened;

            if (help_opened) {
                was_running_when_opened = field_logic.running;
                field_logic.running = false;
                help_menu.SetActive(true);
            } else {
                field_logic.running = was_running_when_opened;
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
            field_logic.running = !field_logic.running;
            statusbar_ctl.UpdateStatusBar();
        }

        /* Camera movements */
        HandleCameraMoves();

        /* Switch user */
        if (Input.GetKeyDown(KeyCode.U)) {
            user_ctl.SwitchUser();
            statusbar_ctl.UpdateStatusBar();
        }

        /* Sim speed */
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals)) {
            field_logic.SpeedUpSim();
            statusbar_ctl.UpdateStatusBar();
        }
        if (Input.GetKeyDown(KeyCode.Minus)) {
            field_logic.SlowDownSim();
            statusbar_ctl.UpdateStatusBar();
        }

        /* Ending Game */
        if (Input.GetKeyDown(KeyCode.End)) {
            field_logic.EndGame();
        }
    }

    void HandleCameraMoves() {
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
