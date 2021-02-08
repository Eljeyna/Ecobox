using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class QuestTasksDatabase
{
    public static Dictionary<int, QuestTasks> allTasks = new Dictionary<int, QuestTasks>();
    public static AsyncOperationHandle<IList<QuestTasks>> handle;

    public static async Task Initialize()
    {
        handle = Addressables.LoadAssetsAsync<QuestTasks>("questTasks", null);
        await handle.Task;
    }

    public static void OnLoad()
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Items not loaded!");
            return;
        }
        
        foreach (var task in handle.Result)
        {
            allTasks.Add(task.id, task);
        }
    }

    public static QuestTasks GetTask(int id)
    {
        if (allTasks.TryGetValue(id, out QuestTasks value))
        {
            return value;
        }

        return null;
    }
}
