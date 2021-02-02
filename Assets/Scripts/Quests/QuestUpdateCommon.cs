public class QuestUpdateCommon : QuestUpdate
{
    public override void UpdateQuest()
    {
        if (quest == null)
        {
            quest = GameDirector.Instance.GetQuest(id);
        }

        if (quest != null)
        {
            GameDirector.Instance.UpdateQuest(quest);
        }
    }

    public override void UpdateQuest(int nextTask)
    {
        if (quest == null)
        {
            quest = GameDirector.Instance.GetQuest(id);
        }

        if (quest != null)
        {
            GameDirector.Instance.UpdateQuest(quest, nextTask);
        }
    }
}
