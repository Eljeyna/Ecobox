using UnityEngine;
using TMPro;
using System.Text;

public class SliderUpdateCommon : MonoBehaviour
{
    public TMP_Text amount;

    private StringBuilder sb = new StringBuilder();

    public void Use(float value)
    {
        sb.Append(0.2f + (0.2f * value));
        amount.text = sb.ToString();
        sb.Clear();
    }
}
