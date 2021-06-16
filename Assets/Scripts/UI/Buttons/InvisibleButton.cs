using UnityEngine;

public class InvisibleButton : MonoBehaviour
{
    public void Use()
    {
        if (GameDirector.Instance && GameDirector.Instance.dialogue)
        {
            GameDirector.Instance.dialogue.SetDialogue(GameDirector.Instance.dialogue._currentLine + 1);
            return;
        }
        
        if (BriefingSystem.Instance && BriefingSystem.Instance.readyForInput)
        {
            BriefingSystem.Instance.UpdateBriefing(BriefingSystem.Instance.currentBriefing + 1);
        }
    }
}
