using UnityEngine;

public class QuestDeleteAfterLoad : MonoBehaviour, IAfterSaveState
{
    public int questID;
    public int task;
    
    public void Load()
    {
        if (GameDirector.Instance.GetQuest(questID).currentTask != task)
        {
            if (TryGetComponent(out QuestUpdateOnDestroy script))
            {
                gameObject.SetActive(false);
            }
            
            Destroy(gameObject);
        }
    }
}
