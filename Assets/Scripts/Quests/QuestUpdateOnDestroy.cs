public class QuestUpdateOnDestroy : QuestUpdateCommon
{
    private void OnDestroy()
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }
        
        UpdateQuest();
    }
}
