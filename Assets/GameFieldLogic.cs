using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldLogic : MonoBehaviour
{
    /* Serializable fields */
    [SerializeField] public GameObject tile_prefab;
    [SerializeField] public int gen_h_size;
    [SerializeField] public int gen_w_size;

    /* Sim status */
    [NonSerialized] public float physics_period = 1f /* secs */;
    [NonSerialized] public bool running = false;
    [NonSerialized] public bool finished = false;

    /* Private fields */
    private float passed_time = 0.0f;

    private int h_count = 0;
    private int w_count = 0;

    private List<List<TileLogic>> tiles = new();
    private List<List<TileState>> next_state_tmp = new();

    /* References to other components */
    private StatusBar statusbar;
    private UserLogic user_logic;

    void Start()
    {
        statusbar = FindObjectOfType<StatusBar>();
        user_logic = FindObjectOfType<UserLogic>();

        /* for each row... */
        for (var i = -gen_h_size / 2; i <= gen_h_size / 2; ++i) {
            h_count++;
            w_count = 0;
            List<TileLogic> row_tile_list = new();
            List<TileState> row_state_list = new();

            /* .. and each column ... */
            for (var j = -gen_w_size / 2; j <= gen_w_size / 2; ++j) {
                w_count++;

                /* place tile and extract it's component */
                var tile_game_obj = Instantiate(tile_prefab, new Vector3(j, 0.5f, i), Quaternion.identity);
                var tile_logic = tile_game_obj.GetComponent<TileLogic>();
                /* Supply pointers to other componets */
                /* Reason: Calling FindComponent in each tile is slow as hell */
                tile_logic.field_logic = this;
                tile_logic.statusbar = statusbar;
                tile_logic.user_logic = user_logic;

                row_tile_list.Add(tile_logic);
                row_state_list.Add(new TileState(false, 0));
            }
            tiles.Add(row_tile_list);
            next_state_tmp.Add(row_state_list);
        }
        statusbar.UpdateStatusBar();
    }

    void Update() {
        passed_time += Time.deltaTime;

        if (passed_time >= physics_period) {
            passed_time -= physics_period;

            if (running && !finished) {
                UpdateField();
                statusbar.UpdateStatusBar();
                RenderField();
            }
        }
    }

    public void SpeedUpSim() {
        physics_period *= 1.1f;
        physics_period = Math.Min(physics_period, 3f);
    }

    public void SlowDownSim() {
        physics_period /= 1.1f;
        physics_period = Math.Max(physics_period, 0.05f);
    }

    public void EndGame() {
        user_logic.EndGame();
        finished = true;
        statusbar.UpdateStatusBar();
    }

    private void UpdateField() {
        /* For each cell ... */
        for (int h = 0; h < h_count; ++h) {
            for (int w = 0; w < w_count; ++w) {
                int alive_cnt = 0;
                int user1_cnt = 0;

                /* ... compute alive neighbours ... */
                for (int delta_h = -1; delta_h <= 1; ++delta_h) {
                    for (int delta_w = -1; delta_w <= 1; ++delta_w) {
                        /* ... while ignoring ourself */
                        if (delta_h == 0 && delta_w == 0) {
                            continue;
                        }

                        TileState state = GetTileState(h + delta_h, w + delta_w);
                        alive_cnt += state.alive ? 1 : 0;
                        user1_cnt += (state.alive && state.user_id == 1) ? 1 : 0;
                    }
                }

                int user2_cnt = alive_cnt - user1_cnt;

                /* and then decide new state */
                TileState cur_state = GetTileState(h, w);
                TileState new_state = cur_state;

                /* Choose new user */
                if (user1_cnt > user2_cnt) {
                    new_state.user_id = 1;
                } else if (user2_cnt > user1_cnt) {
                    new_state.user_id = 2;
                }

                /* Choose new alivness */
                if (cur_state.alive && alive_cnt < 2) {
                    new_state.alive = false;
                } else if (cur_state.alive && alive_cnt > 3) {
                    new_state.alive = false;
                } else if (!cur_state.alive && alive_cnt == 3) {
                    new_state.alive = true;
                }
                
                /* Notify User component about this change */
                user_logic.SimUpdateField(cur_state, new_state);

                next_state_tmp[h][w] = new_state;
            }
        }
    }

    private void RenderField() {
        /* For each cell ... */
        for (int h = 0; h < h_count; ++h) {
            for (int w = 0; w < w_count; ++w) {
                tiles[h][w].CurState = next_state_tmp[h][w];
            }
        }
    }

    private TileState GetTileState(int h, int w) {
        if (h > 0 && w > 0 && h < h_count && w < w_count) {
            return tiles[h][w].CurState;
        } else {
            return new TileState(false, 0);
        }
    }
}
