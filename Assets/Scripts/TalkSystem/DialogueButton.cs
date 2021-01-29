using UnityEngine;
using TMPro;

public class DialogueButton : MonoBehaviour
{
    public int line;

    public TMP_Text text;

    public void Use()
    {
        GameDirector.Instance.dialogue.SetDialogue(line);
    }
}
