using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum State {
    Alive,
    Dead
}

public class FieldLogic : MonoBehaviour
{
    [SerializeField] public GameObject tile_prefab;
    [SerializeField] public int gen_h_size;
    [SerializeField] public int gen_w_size;

    private float physics_period = 1f /* secs */;
    private float time = 0.0f;

    private int h_count = 0;
    private int w_count = 0;

    private List<List<TileLogic>> tiles = new();
    private List<List<State>> next_state_tmp = new();

    [NonSerialized] public bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = -gen_h_size / 2; i <= gen_h_size / 2; ++i) {
            h_count++;
            w_count = 0;
            List<TileLogic> row_tile_list = new();
            List<State> row_state_list = new();

            for (var j = -gen_w_size / 2; j <= gen_w_size / 2; ++j) {
                w_count++;

                var tile_game_obj = Instantiate(tile_prefab, new Vector3(j, 0.5f, i), Quaternion.identity);
                row_tile_list.Add(tile_game_obj.GetComponent<TileLogic>());
                row_state_list.Add(State.Dead);
            }
            tiles.Add(row_tile_list);
            next_state_tmp.Add(row_state_list);
        }
    }

    void Update() {
        time += Time.deltaTime;

        if (time >= physics_period) {
            time -= physics_period;

            if (running) {
                UpdateField();
                RenderField();
            }
        }
    }

    private void UpdateField() {
        /* For each cell ... */
        for (int h = 0; h < h_count; ++h) {
            for (int w = 0; w < w_count; ++w) {
                int alive_cnt = 0;

                /* ... compute alive neighbours ... */
                for (int delta_h = -1; delta_h <= 1; ++delta_h) {
                    for (int delta_w = -1; delta_w <= 1; ++delta_w) {
                        /* ... while ignoring ourself */
                        if (delta_h == 0 && delta_w == 0) {
                            continue;
                        }

                        State state = GetTileState(h + delta_h, w + delta_w);
                        alive_cnt += state == State.Alive ? 1 : 0;
                    }
                }

                /* and then decide new state */
                State cur_state = GetTileState(h, w);
                State new_state = cur_state;
                if (cur_state == State.Alive && alive_cnt < 2) {
                    new_state = State.Dead;
                } else if (cur_state == State.Alive && alive_cnt > 3) {
                    new_state = State.Dead;
                } else if (cur_state == State.Dead && alive_cnt == 3) {
                    new_state = State.Alive;
                }
                next_state_tmp[h][w] = new_state;
            }
        }
    }

    void RenderField() {
        /* For each cell ... */
        for (int h = 0; h < h_count; ++h) {
            for (int w = 0; w < w_count; ++w) {
                tiles[h][w].CurState = next_state_tmp[h][w];
            }
        }
    }

    private State GetTileState(int h, int w) {
        if (h > 0 && w > 0 && h < h_count && w < w_count) {
            return tiles[h][w].CurState;
        } else {
            return State.Dead;
        }
    }
}
