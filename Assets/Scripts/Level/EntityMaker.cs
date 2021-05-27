using UnityEngine;
using UnityEngine.AddressableAssets;
using static Game;

[RequireComponent(typeof(CircleCollider2D))]
public class EntityMaker : MonoBehaviour
{
    public bool isTrigger;
    [Range(1, 4)] public int amountMin;
    [Range(1, 8)] public int amountMax;

    public AssetReference[] entities;
    public Transform[] positions;
    public Transform target;

    private float distance;
    
    private GameObject[] createdObjects;

    /*private void Update()
    {
        if (isPause)
        {
            return;
        }
        
        if (!GameDirector.Instance)
        {
            this.enabled = false;
            return;
        }

        distance = Vector2.Distance(Player.Instance.transform.position, transform.position);
        if (distance > distanceFade)
        {
            this.enabled = false;
            return;
        }

        if (isTrigger && distance <= minDistance && (!target || target == transform))
        {
            Player.Instance.fightCount++;
            target = Player.Instance.transform;
            UpdateTarget();
        }
        
        if (!target || target == transform)
        {
            return;
        }
        
        distance = Vector2.Distance(target.position, transform.position);
        if (distance > maxDistance)
        {
            Player.Instance.fightCount--;
            target = transform;
            UpdateTarget();
        }
        
        if (transform.childCount == 0)
        {
            this.enabled = false;
        }
    }

    public void UpdateTarget()
    {
        if (transform.childCount == 0)
        {
            return;
        }

        if (target == transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out AIEntity aiEntityScript))
                {
                    aiEntityScript.isEnemy = false;
                    aiEntityScript.aiEntity.target = positions[i];
                    aiEntityScript.aiPath.endReachedDistance = aiEntityScript.defaultEndReachedDistance;
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out AIEntity aiEntityScript))
                {
                    aiEntityScript.UpdateTarget(target);
                }
            }
        }
    }

    public async void SpawnEntities()
    {
        if (ReferenceEquals(createdObjects, null))
        {
            createdObjects = new GameObject[amountMax];
        }

        int amount = amountMax - 1;
        for (int i = 0; i < entities.Length; i++)
        {
            if (amount == 0)
            {
                break;
            }
            
            GetRandom();

            int count = Mathf.RoundToInt(amount * random + 1);
            for (int j = 0; j < count; j++)
            {
                createdObjects[j] = await Addressables.InstantiateAsync(entities[i], transform, true).Task;

                if (createdObjects[j].TryGetComponent(out AIEntity aiEntityScript))
                {
                    aiEntityScript.aiEntity.target = positions[i];
                    aiEntityScript.aiPath.endReachedDistance = aiEntityScript.defaultEndReachedDistance;
                }
            }

            amount -= count;
        }
    }

    public void DeleteEntities()
    {
        if (createdObjects.Length > 0)
        {
            for (int i = 0; i < createdObjects.Length; i++)
            {
                if (createdObjects[i])
                {
                    Addressables.ReleaseInstance(createdObjects[i]);
                }
            }
        }
    }

    private void OnEnable()
    {
        SpawnEntities();
    }

    private void OnDisable()
    {
        if (Player.Instance && target == Player.Instance.transform)
        {
            Player.Instance.fightCount--;
        }

        isTrigger = false;
        target = transform;

        if (transform.childCount == 0)
        {
            return;
        }
        
        DeleteEntities();
    }

    private void OnValidate()
    {
        if (amountMax < amountMin)
        {
            amountMax = amountMin;
            amountMin--;
        }

        if (!target)
        {
            target = transform;
        }
    }*/
}
