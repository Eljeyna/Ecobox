using System.Collections.Generic;
using UnityEngine;

public class SpawnerSnake : MonoBehaviour
{
    public bool spawn;
    public GameObject[] wavePrefabs;
    public List<GameObject> waveEntities;
    public int currentWaveCount;
    public float checkRadius = 1f;
    public BoxCollider2D spawnArea;
    public float waitingTime;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (waitingTime > Time.time)
            return;

        if (spawn && currentWaveCount < GameDirector3D.maxWave)
        {
            for (int i = 0; i < (int)Random.Range(1f, GameDirector3D.maxWave - currentWaveCount); i++)
            {
                int random = (int)Random.Range(0f, wavePrefabs.Length);

                Vector3 pos = new Vector3(
                        Random.Range(spawnArea.transform.position.x - spawnArea.bounds.extents.x, spawnArea.bounds.extents.x),
                        Random.Range(spawnArea.transform.position.y + spawnArea.bounds.extents.y + spawnArea.size.y, spawnArea.bounds.extents.y),
                        0f
                );

                float distance = Vector3.Distance(GameDirectorSnake.GetPlayer().position, pos);

                Collider[] checkResult = Physics.OverlapSphere(pos, checkRadius);

                if (checkResult.Length == 0 && distance > 4f)
                {
                    pos.y -= spawnArea.size.y;
                    waveEntities.Add(Instantiate(wavePrefabs[random], pos, Quaternion.identity, transform));
                    waitingTime = Time.time + Random.Range(0f, 5f);

                    currentWaveCount++;
                }
            }
        }
    }
}
