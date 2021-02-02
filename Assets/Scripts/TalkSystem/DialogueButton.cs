using UnityEngine;
using TMPro;

public class DialogueButton : MonoBehaviour
{
    public int line;

    public TMP_Text text;

    [HideInInspector] public int id;

    public void Use()
    {
        GameDirector.Instance.dialogue.SetDialogue(id, line);
    }
}
