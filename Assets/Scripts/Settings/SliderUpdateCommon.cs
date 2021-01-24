using UnityEngine;
using TMPro;

public class SliderUpdateCommon : MonoBehaviour
{
    public TMP_Text amount;

    public void Use(float value)
    {
        amount.text = ($"{0.2f + (0.2f * value)}");
    }
}
