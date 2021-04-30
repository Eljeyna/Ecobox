public class QuestUpdateCommon : QuestUpdate
{
    public override void UpdateQuest(int nextTask)
    {
        GameDirector.Instance.UpdateQuest(id, nextTask);
    }
}
