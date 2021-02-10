using System.Threading.Tasks;

public enum QuestState
{
    Received = 0,
    Active,
}

public class Quest
{
    public string id;
    public QuestState state;
    public int currentTask;

    public QuestTasks tasks;

    public Quest(string id)
    {
        this.id = id;
        this.state = QuestState.Received;
        this.currentTask = 0;
    }

    public Quest(string id, QuestState state)
    {
        this.id = id;
        this.state = state;
        this.currentTask = 0;
    }

    public Quest(string id, QuestState state, int currentTask)
    {
        this.id = id;
        this.state = state;
        this.currentTask = currentTask;
    }

    public async Task Initialize()
    {
        this.tasks = await Database.GetItem<QuestTasks>(id);
    }
}
