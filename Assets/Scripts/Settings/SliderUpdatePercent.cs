using UnityEngine;
using TMPro;

public class SliderUpdatePercent : MonoBehaviour
{
    public TMP_Text percent;

    public void Use(float value)
    {
        percent.text = Mathf.RoundToInt(value / 80f * 100f + 100f).ToString();
    }
}
