using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position + new Vector3(1f, 1.5f, -1f);
    }
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
