public enum QuestState
{
    Received = 0,
    Active,
}

public class Quest
{
    public int id;
    public QuestState state;
    public int currentTask;

    public QuestTasks tasks;

    public Quest(int id)
    {
        this.id = id;
        this.state = QuestState.Received;
        this.currentTask = 0;
        this.tasks = QuestTasksDatabase.GetTask(id);
    }

    public Quest(int id, QuestState state)
    {
        this.id = id;
        this.state = state;
        this.currentTask = 0;
        this.tasks = QuestTasksDatabase.GetTask(id);
    }

    public Quest(int id, QuestState state, int currentTask)
    {
        this.id = id;
        this.state = state;
        this.currentTask = currentTask;
        this.tasks = QuestTasksDatabase.GetTask(id);
    }
}
