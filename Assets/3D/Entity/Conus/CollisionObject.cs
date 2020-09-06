using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    [HideInInspector] public List<Collider> objectCollision = new List<Collider>();
}
