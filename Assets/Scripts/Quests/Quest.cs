using UnityEngine.AddressableAssets;

public class Quest
{
    public string id;
    public int currentTask;

    public QuestTasks tasks;

    public Quest(string id)
    {
        this.id = id;
        this.currentTask = 0;
        this.tasks = new QuestTasks(id, currentTask);
    }

    public Quest(string id, int currentTask)
    {
        this.id = id;
        this.currentTask = currentTask;
        this.tasks = new QuestTasks(id, currentTask);
    }

    public void Complete()
    {
        Addressables.Release(tasks.questInitialize);
    }
}
