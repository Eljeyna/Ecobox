using System.Collections.Generic;
using UnityEngine;

public class Spawner3D : MonoBehaviour
{
    public bool spawn;
    public GameObject[] wavePrefabs;
    public List<GameObject> waveEntities;
    public int currentWaveCount;
    public float checkRadius = 1f;

    private void Update()
    {
        if (spawn && currentWaveCount < GameDirector3D.maxWave)
        {
            for (int i = 0; i < (int)Random.Range(1f, 8f); i++)
            {
                int random = (int)Random.Range(0f, 1f);
                /*Collider[] checkResult = Physics.OverlapSphere(targetPosition, checkRadius);
                if (checkResult.Length == 0)
                {
                    waveEntities.Add(Instantiate(wavePrefabs[random], ));
                }*/
            }
        }
    }
}
