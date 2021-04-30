using UnityEngine;

public class LoadingInfinity : MonoBehaviour
{
    public float speed;
    public float radius;

    private float evaluateTime;

    private Vector3 newPosition;
    private RectTransform rectTransform;

    private void Start()
    {
        if (TryGetComponent(out RectTransform newRectTransform))
        {
            rectTransform = newRectTransform;
        }
    }

    private void Update()
    {
        evaluateTime += Time.unscaledDeltaTime * speed;

        newPosition.x = Mathf.Cos(evaluateTime / 2f) * radius * 2f;
        newPosition.y = Mathf.Sin(evaluateTime) * radius;
        
        rectTransform.localPosition = newPosition;
    }
}
