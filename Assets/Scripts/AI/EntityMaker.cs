using UnityEngine;
using static StaticGameVariables;

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

    private void Update()
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
