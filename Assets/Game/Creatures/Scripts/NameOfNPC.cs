using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameOfNPC : MonoBehaviour
{
    private TMP_Text name_box;
    private IsTalking nameNPC;

    void Start()
    {
        name_box = gameObject.GetComponent<TMP_Text>();
        if (name_box != null)
        {
            nameNPC = gameObject.transform.parent.GetComponent<IsTalking>();
            if (name != null)
            {
                name_box.text = nameNPC.speakerName;
            }
        }
    }
}
