using UnityEngine;

public class MinimapUpdate : MonoBehaviour
{
    public Canvas canvas;
    
    private void LateUpdate()
    {
        canvas.enabled = false;
        canvas.enabled = true;
    }
}
