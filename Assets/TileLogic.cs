using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileLogic : MonoBehaviour
{
    const int MAX_DEAD_PHASE = 3;

    Material my_material_;
    private State _cur_state;
    private int dead_phase = MAX_DEAD_PHASE;
    private Color cell_color = new(); 
    private FieldLogic field_logic;

    public Users owner = Users.User1;
    public State CurState {
        get => _cur_state;
        set {
            _cur_state = value;
            if (value == State.Alive) {
                cell_color.r = cell_color.g = cell_color.b = 1.0f;
                my_material_.color = cell_color;
                dead_phase = 0;
            } else if (value == State.Dead) {
                float color_val = 0.01f + 0.6f * (MAX_DEAD_PHASE - dead_phase) / MAX_DEAD_PHASE;
                cell_color.r = cell_color.g = cell_color.b = color_val;
                my_material_.color = cell_color;

                if (dead_phase < MAX_DEAD_PHASE) dead_phase++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        my_material_ = gameObject.GetComponent<Renderer>().material;
        cell_color.a = 1;
        CurState = State.Dead;
        field_logic = GameObject.Find("Script Runner").GetComponent<FieldLogic>();
    }

    void OnMouseEnter() {
        if (!Input.GetMouseButton(0) || field_logic.running) {
            return;
        }

        CurState = State.Alive;
    }
}
