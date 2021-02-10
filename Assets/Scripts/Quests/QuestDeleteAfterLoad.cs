using UnityEngine;

public class QuestDeleteAfterLoad : MonoBehaviour, IAfterSaveState
{
    public string questID;
    public int task;
    
    public void Load()
    {
        if (GameDirector.Instance.GetQuest(questID).currentTask != task)
        {
            if (TryGetComponent(out QuestUpdateOnDestroy script))
            {
                script.dontUpdate = true;
            }
            
            Destroy(gameObject);
        }
    }
}
