using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private TextMeshProUGUI statusbar;
    private UserLogic user_logic;
    private FieldLogic field_logic;

    // Start is called before the first frame update
    void Start()
    {
        statusbar = gameObject.GetComponent<TextMeshProUGUI>();
        user_logic = FindObjectOfType<UserLogic>();
        field_logic = FindObjectOfType<FieldLogic>();
        UpdateStatusBar();
    }

    public void UpdateStatusBar() {
        string username = user_logic.cur_user == Users.User1 ? "User1" : "User2";
        string sim_status = field_logic.running ? "running" : "paused";
        statusbar.text = String.Format("[{0}] [Sim: {1}] ", username, sim_status);
    }
}
