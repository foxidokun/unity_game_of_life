using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    /* References to other components */
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

    public void UpdateStatusBar()
    {
        if (field_logic.finished) {
            if ( user_logic.won_user == 0) {
                statusbar.text = string.Format("Equal Points");    
            } else {
                statusbar.text = string.Format("User {0} won", user_logic.won_user);
            }
            return;
        }

        string sim_status = field_logic.running ? "running" : "paused";
        statusbar.text = string.Format("[User: #{0}] [Sim: {1}] [Points: {2:0} vs {3:0}] [Simulate speed: {4:##.##} secs/tick]",
            user_logic.cur_user, sim_status, user_logic.points[1], user_logic.points[2], field_logic.physics_period);
    }
}
