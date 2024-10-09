public struct TileState {
    public TileState(bool alive_, byte user_id_) {
        alive = alive_;
        user_id = user_id_;
    }

    public bool alive;
    public byte user_id;
}