public static class Tags
{
    [System.Flags]
    public enum EntityTags : int
    {
        FL_PLAYER = (1 << 0),
        FL_ENEMY = (1 << 1),
        FL_PICKUP = (1 << 2),
        FL_CONUS = (1 << 3),

        FL_TRASHBOX = (1 << 4),
    }
}
