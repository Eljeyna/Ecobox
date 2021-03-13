using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnerEnemies : MonoBehaviour
{
    public SpawnerEnemies Instance { get; private set; }
    
    public AssetReference[] entities;

    private Block[] points;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn()
    {
        for (int i = 0; i < points.Length; i++)
        {
            // TODO : Spawn enemies here
        }
    }

    public void SetPoints(Block[] points)
    {
        this.points = points;
    }

    public void GeneratePoints()
    {
        points = new Block[10];

        for (int i = 0; i < points.Length; i++)
        {
            //points[i].topLeft = 
        }
    }
}
