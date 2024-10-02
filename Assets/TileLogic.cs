using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    Material my_material_;
    private State _cur_state;

    public State CurState {
        get => _cur_state;
        set {
            _cur_state = value;
            if (value == State.Alive) {
                my_material_.color = new Color(1.0f, 1.0f, 1.0f, 1);
            } else if (value == State.Dead) {
                my_material_.color = new Color(0.1f, 0.1f, 0.1f, 1);
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
}
