public class QuestUpdateCommon : QuestUpdate
{
    public override void UpdateQuest()
    {
        GameDirector.Instance.UpdateQuest(id);
    }

    public override void UpdateQuest(int nextTask)
    {
        GameDirector.Instance.UpdateQuest(id, nextTask);
    }
}
