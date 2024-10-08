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

    public State CurState {
        get => _cur_state;
        set {
            _cur_state = value;
            if (value == State.Alive) {
                my_material_.color = new Color(1.0f, 1.0f, 1.0f, 1);
                dead_phase = 0;
            } else if (value == State.Dead) {
                float color_x = 0.01f + 0.6f * (MAX_DEAD_PHASE - dead_phase) / MAX_DEAD_PHASE;
                my_material_.color = new Color(color_x, color_x, color_x, 1);

                if (dead_phase < MAX_DEAD_PHASE) dead_phase++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        my_material_ = gameObject.GetComponent<Renderer>().material;
        Debug.Assert(my_material_ != null);
        CurState = State.Dead;
    }

    void OnMouseEnter() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (!Input.GetMouseButton(0)) {
            return;
        }

        CurState = State.Alive;
    }
}
