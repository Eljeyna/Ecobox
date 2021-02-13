public class QuestUpdateOnDestroy : QuestUpdateCommon
{
    private void OnDestroy()
    {
        UpdateQuest(taskID);
    }
}
