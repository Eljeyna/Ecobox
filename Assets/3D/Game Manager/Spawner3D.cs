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
    public Transform entitiesGroup;
    public float waitingTime;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (waitingTime > Time.time)
            return;

        if (spawn && currentWaveCount < GameDirector3D.maxWave)
        {
            for (int i = 0; i < (int)Random.Range(1f, GameDirector3D.maxWave); i++)
            {
                int random = (int)Random.Range(0f, wavePrefabs.Length - 1);
                int attempts = 0;

                Vector3 pos = new Vector3(
                        Random.Range(spawnArea.transform.position.x, spawnArea.transform.position.x + spawnArea.size.x),
                        0,
                        Random.Range(spawnArea.transform.position.z, spawnArea.transform.position.z + spawnArea.size.z)
                );

                Collider[] checkResult = Physics.OverlapSphere(pos, checkRadius);

                while (checkResult.Length > 0 && attempts < 10)
                {
                    pos = new Vector3(
                        Random.Range(spawnArea.transform.position.x, spawnArea.transform.position.x + spawnArea.size.x),
                        0,
                        Random.Range(spawnArea.transform.position.z, spawnArea.transform.position.z + spawnArea.size.z)
                    );

                    if (checkResult.Length == 0)
                    {
                        waveEntities.Add(Instantiate(wavePrefabs[random], pos, Quaternion.identity, entitiesGroup));
                        currentWaveCount++;
                        waitingTime = Time.time + Random.Range(0f, 2f);
                        continue;
                    }

                    checkResult = Physics.OverlapSphere(pos, checkRadius);
                    attempts++;
                }
            }
        }
    }
}
