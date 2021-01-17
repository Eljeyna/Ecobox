using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    public int index;
    public float damage;
    public float speed;
    public float timeFade;
    public float radius;
}
