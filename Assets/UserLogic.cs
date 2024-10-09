using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLogic : MonoBehaviour
{
    [NonSerialized] public byte cur_user = 1;
    [NonSerialized] public float[] points = new float[3]{0f, STARTUP_POINTS, STARTUP_POINTS};
    public byte won_user;

    private const float CREATE_COST = 5;
    private const float STOLE_COST = 10;
    private const float STARTUP_POINTS = 50;
    private const float NEW_AWARD = 0.2f;
    private const float STOLEN_AWARD = 1f;
    private const float KEEP_AWARD = 0.1f;
    private const float ENDGAME_THRESHOLD = 500f;

    void Start() {
        FindObjectOfType<StatusBar>().UpdateStatusBar();
    }

    public void SwitchUser() {
        cur_user = (byte) ((cur_user == 1) ? 2 : 1);
    }

    public bool UserRequestField(TileState prev_state, TileState new_state) {
        /* probably missclick */
        if (prev_state.alive && prev_state.user_id == new_state.user_id) {
            return true;
        }

        if (prev_state.alive && prev_state.user_id != new_state.user_id) {
            if (points[new_state.user_id] >= STOLE_COST) {
                points[new_state.user_id] -= STOLE_COST;
                return true;
            }
            return false;
        }

        /* prev state is dead */
        if (points[new_state.user_id] >= CREATE_COST) {
            points[new_state.user_id] -= CREATE_COST;
            return true;
        }

        return false;
    }

    public void SimUpdateField(TileState prev_state, TileState new_state) {
        /* No reward for dead cells*/
        if (!new_state.alive) {
            return;
        }

        if (prev_state.alive && prev_state.user_id == new_state.user_id) {
            points[new_state.user_id] += KEEP_AWARD;
        }

        if (prev_state.alive && prev_state.user_id != new_state.user_id) {
            points[new_state.user_id] += STOLEN_AWARD;
        }

        if (!prev_state.alive) {
            points[new_state.user_id] += NEW_AWARD;
        }
    }
    public bool CheckEndGame() {
        for (byte i = 1; i <= 2; ++i ) {
            if (points[i] > ENDGAME_THRESHOLD) {
                won_user = i;
                return true;
            }
        }

        return false;
    }
}
