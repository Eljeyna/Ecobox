using System.Collections;
using UnityEngine;

public class Fadeout : MonoBehaviour
{
    public IEnumerator Fade(SpriteRenderer rend, float speed)
    {
        Color color = rend.color;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / speed;
            rend.color = color;

            if (color.a <= 0f)
            {
                color.a = 0f;
            }

            yield return null;
        }

        rend.color = color;
    }
}
