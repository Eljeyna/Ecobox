using UnityEngine;

public class TrashTag : MonoBehaviour
{
    [System.Flags]
    public enum TrashTags : int
    {
        FL_RED = (1 << 0),
        FL_BLUE = (1 << 1),
        FL_WHITE = (1 << 2),
    }
}
