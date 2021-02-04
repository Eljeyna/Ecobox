using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public readonly Dictionary<ScriptableObjectBuff, Buff> buffs = new Dictionary<ScriptableObjectBuff, Buff>();
    private List<Buff> buffList = new List<Buff>();

    void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        foreach (Buff buff in buffs.Values)
        {
            if (buff.isPersist)
            {
                continue;
            }
            
            buff.Tick(Time.deltaTime);
            if (buff.isFinished)
            {
                buffList.Add(buff);
            }
        }

        if (buffList.Count > 0)
        {
            for (int i = buffList.Count - 1; i >= 0; i--)
            {
                buffs.Remove(buffList[i].buffData);
            }
            
            buffList.Clear();
        }
    }

    public void AddBuff(Buff buff)
    {
        if (buffs.ContainsKey(buff.buffData))
        {
            buffs[buff.buffData].Activate();
        }
        else
        {
            buffs.Add(buff.buffData, buff);
            buff.Activate();
        }
    }
    
    public void RemoveBuff(Buff buff)
    {
        if (buffs.ContainsKey(buff.buffData))
        {
            buff.End();
            buffs.Remove(buff.buffData);
        }
    }
}
