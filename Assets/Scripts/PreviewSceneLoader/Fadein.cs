using System.Collections;
using UnityEngine;

public class Fadein : MonoBehaviour
{
    public IEnumerator Fade(SpriteRenderer rend, float speed)
    {
        Color color = rend.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / speed;
            rend.color = color;

            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
        rend.color = color;
    }
}
