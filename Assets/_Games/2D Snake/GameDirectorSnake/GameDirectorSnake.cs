using UnityEngine;

public static class GameDirectorSnake
{
    private static GameObject player;

    public static Transform GetPlayer()
    {
        if (player == null)
            player = GameObject.Find("Snake");

        return player.transform;
    }
}
