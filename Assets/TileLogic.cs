using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileLogic : MonoBehaviour
{
    const int MAX_DEAD_PHASE = 3;

    Material my_material_;
    private TileState cur_state;
    private int dead_phase = MAX_DEAD_PHASE;
    public FieldLogic field_logic;
    public UserLogic user_logic;
    public StatusBar statusbar;

    public TileState CurState {
        get => cur_state;
        set {
            cur_state = value;
            UpdateColor();
        }
    }

    // Start is called before the first frame update
    void Start() {
        my_material_ = gameObject.GetComponent<Renderer>().material;
        CurState = new TileState(false, 0);
    }

    void OnMouseDown() {
        HandleMouseInput();
    }

    void OnMouseEnter() {
        HandleMouseInput();
    }

    void HandleMouseInput() {
        if (!Input.GetMouseButton(0) || field_logic.running) {
            return;
        }

        TileState requested_state = new TileState(true, user_logic.cur_user);

        if (user_logic.UserRequestField(cur_state, requested_state)) {
            CurState = requested_state;
        }

        statusbar.UpdateStatusBar();
    }

    void UpdateColor() {
        if (cur_state.alive) {
            my_material_.color = ChooseColor(1.0f);
            dead_phase = 0;
        } else {
            float factor = 0.01f + 0.6f * (MAX_DEAD_PHASE - dead_phase) / MAX_DEAD_PHASE;
            my_material_.color = ChooseColor(factor);

            if (dead_phase < MAX_DEAD_PHASE) dead_phase++;
        }
    }

    Color ChooseColor(float factor) {
        Color res;
        res.r = (cur_state.user_id == 1) ? factor : factor * 0.6f;
        res.g = factor * 0.6f;
        res.b = (cur_state.user_id == 2) ? factor : factor * 0.6f;
        res.a = 1.0f;
        return res;
    }
}