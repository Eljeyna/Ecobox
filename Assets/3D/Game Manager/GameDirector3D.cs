using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector3D
{
    public static GameObject player;
    public static int maxWave = 8;
    public static int maxEntities = 40;

    public static Transform GetPlayer()
    {
        if (player == null)
            player = GameObject.Find("Synthea");

        return player.transform;
    }

    public static void StartGame()
    {

    }

    IEnumerator Spawner()
    {
        yield return null;
    }
}
