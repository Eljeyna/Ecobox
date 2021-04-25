using UnityEngine;

public class TutorialStart : DialogueScript
{
    public override void Use()
    {
        GameObject levelObject = GameObject.Find("_LEVEL");

        if (levelObject.TryGetComponent(out Tutorial tutorial))
        {
            tutorial.enabled = true;
        }
    }
}
