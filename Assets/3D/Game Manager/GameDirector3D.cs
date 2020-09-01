using UnityEngine;

public class GameDirector3D
{
    public static int maxWave = 8;
    public static int maxEntities = 40;

    private static GameObject player;

    public static Transform GetPlayer()
    {
        if (player == null)
            player = GameObject.Find("Synthea");

        return player.transform;
    }

    public static void StartGame()
    {

    }
}
