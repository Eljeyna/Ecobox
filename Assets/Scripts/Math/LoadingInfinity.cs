using UnityEngine;

public class LoadingInfinity : MonoBehaviour
{
    public float speed;
    public float radius;

    private float evaluateTime;
    private Vector3 newPosition;

    void Update()
    {
        evaluateTime += Time.deltaTime * speed;

        newPosition.x = Mathf.Sin(evaluateTime) * radius;
        newPosition.y = Mathf.Cos(evaluateTime / 2f) * radius;
        
        transform.position = newPosition;
    }
}
