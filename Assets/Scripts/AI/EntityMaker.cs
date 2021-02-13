using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Addressables_Instantiate))]
public class EntityMaker : MonoBehaviour
{
    public bool isTrigger;
    [Range(1, 4)] public int amountMin;
    [Range(1, 8)] public int amountMax;

    public Addressables_Instantiate instantiateScript;
    public Transform target;

    private float distance;
    private const float minDistance = 20f;
    private const float maxDistance = 60f;
    private const float distanceFade = 80f;

    private void Update()
    {
        if (!Player.Instance)
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

        if (isTrigger && Player.Instance && distance <= minDistance && (!target || target == transform))
        {
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
            target = transform;
            UpdateTarget();
        }
    }

    public void UpdateTarget()
    {
        if (transform.childCount > 0)
        {
            if (target == transform)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).TryGetComponent(out AIEntity aiEntityScript))
                    {
                        aiEntityScript.isEnemy = false;
                        aiEntityScript.aiEntity.target = target;
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
    }
    
    public void SpawnEntities()
    {
        instantiateScript.SpawnEntities();
    }

    private void OnEnable()
    {
        SpawnEntities();
    }

    private void OnDisable()
    {
        isTrigger = false;

        if (transform.childCount == 0)
        {
            return;
        }
        
        instantiateScript.DeleteEntities();
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
    }
}
