using UnityEngine;
using TMPro;
using System.Text;

public class SliderUpdatePercent : MonoBehaviour
{
    public TMP_Text percent;

    private StringBuilder sb = new StringBuilder();

    public void Use(float value)
    {
        sb.Append(Mathf.RoundToInt(value / 80f * 100f + 100f));
        percent.text = sb.ToString();
        sb.Clear();
    }
}
