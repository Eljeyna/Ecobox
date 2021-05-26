public class DialogueQuestUpdate : DialogueScript
{
    public string id;
    public int nextTask;
    public override void Use()
    {
        GameDirector.Instance.UpdateQuest(id, nextTask);
    }
}
