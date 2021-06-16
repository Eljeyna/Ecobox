public class DialogueScriptUseArray : DialogueScript
{
    public DialogueScript[] scripts;

    public override void Use()
    {
        for (int i = 0; i < scripts.Length; i++)
        {
            scripts[i].Use();
        }
    }
}
