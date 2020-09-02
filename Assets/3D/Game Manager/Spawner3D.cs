using System.Collections.Generic;
using UnityEngine;

public class Spawner3D : MonoBehaviour
{
    public bool spawn;
    public GameObject[] wavePrefabs;
    public List<GameObject> waveEntities;
    public int currentWaveCount;
    public float checkRadius = 1f;
    public BoxCollider spawnArea;
    public float waitingTime;

    public int allWaveCount;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (allWaveCount >= GameDirector3D.maxEntities)
        {
            GameDirector3D.VictoryGame();
            this.enabled = false;
            return;
        }

        if (waitingTime > Time.time)
            return;

        if (spawn && currentWaveCount < GameDirector3D.maxWave)
        {
            for (int i = 0; i < (int)Random.Range(1f, GameDirector3D.maxWave - currentWaveCount); i++)
            {
                int random = (int)Random.Range(0f, wavePrefabs.Length);

                Vector3 pos = new Vector3(
                        Random.Range(spawnArea.transform.position.x, spawnArea.transform.position.x + spawnArea.size.x),
                        spawnArea.transform.position.y + spawnArea.size.y + checkRadius,
                        Random.Range(spawnArea.transform.position.z, spawnArea.transform.position.z + spawnArea.size.z)
                );

                float distance = Vector3.Distance(GameDirector3D.GetPlayer().position, pos);

                Collider[] checkResult = Physics.OverlapSphere(pos, checkRadius);

                if (checkResult.Length == 0 && distance > 4f)
                {
                    pos.y -= spawnArea.size.y + checkRadius;
                    waveEntities.Add(Instantiate(wavePrefabs[random], pos, Quaternion.identity, transform));
                    waitingTime = Time.time + Random.Range(0f, 5f);

                    currentWaveCount++;
                    allWaveCount++;
                }
            }
        }
    }
}
