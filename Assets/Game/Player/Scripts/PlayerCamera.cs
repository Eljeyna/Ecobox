using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public float smoothing = 5f;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = player.transform.position + offset;
        //transform.position = targetCamPos;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}