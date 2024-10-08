using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    TilePlacer field_object;
    GameObject help_menu;
    private bool help_opened = true;

    private bool was_running_when_opened = false;

    void Start() {
        field_object = GetComponent<TilePlacer>();
        help_menu = GameObject.Find("Help Canvas");
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            field_object.running = !field_object.running;
        }
    }
}
