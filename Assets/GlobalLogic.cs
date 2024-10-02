using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Alive,
    Dead
}

public class TilePlacer : MonoBehaviour
{
    [SerializeField] public GameObject tile_prefab;
    [SerializeField] public int gen_h_size = 2;
    [SerializeField] public int gen_w_size = 2;

    private int h_count = 0;
    private int w_count = 0;

    List<List<TileLogic>> tiles = new();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = -gen_h_size / 2; i <= gen_h_size / 2; ++i) {
            h_count++;
            w_count = 0;
            List<TileLogic> row_list = new();
            for (var j = -gen_w_size / 2; j <= gen_w_size / 2; ++j) {
                w_count++;

                var tile_game_obj = Instantiate(tile_prefab, new Vector3(j, 0.5f, i), Quaternion.identity);
                var tile_logic = tile_game_obj.GetComponent<TileLogic>();
                Debug.Assert(tile_logic != null);
                row_list.Add(tile_logic);
            }
            tiles.Add(row_list);
        }
    }

    void Update() {
        for (int h = 0; h < h_count; ++h) {
            for (int w = 0; w < w_count; ++w) {
                tiles[h][w].CurState = h % 2 == 0 ? State.Alive : State.Dead;
            }
        }
    }
}
