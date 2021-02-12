using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Addressables_Instantiate))]
public class EntityMaker : MonoBehaviour
{
    public Addressables_Instantiate instantiateScript;
    [Range(1, 4)] public int amountMin;
    [Range(1, 8)] public int amountMax;

    private void Awake()
    {
        SpawnEntities();
    }

    public void SpawnEntities()
    {
        instantiateScript.AddressablesInstantiate();
    }
    
    private void OnValidate()
    {
        if (amountMax < amountMin)
        {
            amountMax = amountMin;
            amountMin--;
        }
    }
}
