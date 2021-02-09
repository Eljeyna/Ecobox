public class QuestUpdateOnDestroy : QuestUpdateCommon
{
    public bool dontUpdate = false;
    private void OnDestroy()
    {
        if (dontUpdate)
        {
            return;
        }
        
        UpdateQuest();
    }
}
