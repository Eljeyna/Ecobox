using UnityEngine;

public class InvisibleButton : MonoBehaviour
{
    public void Use()
    {
        GameDirector.Instance.dialogue.SetDialogue(GameDirector.Instance.dialogue._currentLine + 1);
    }
}
