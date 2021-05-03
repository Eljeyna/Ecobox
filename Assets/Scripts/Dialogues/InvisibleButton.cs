using UnityEngine;

public class InvisibleButton : MonoBehaviour
{
    public void Use()
    {
        if (!GameDirector.Instance.dialogue)
        {
            return;
        }

        GameDirector.Instance.dialogue.SetDialogue(GameDirector.Instance.dialogue._currentLine + 1);
    }
}
