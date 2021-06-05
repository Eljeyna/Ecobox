using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public Dictionary<ScriptableObjectBuff, Buff> buffs = new Dictionary<ScriptableObjectBuff, Buff>();
    private List<Buff> buffList = new List<Buff>();

    void Update()
    {
        if (Game.isPause)
        {
            return;
        }

        foreach (Buff buff in buffs.Values)
        {
            if (buff.buffData.isPersist)
            {
                continue;
            }
            
            if (buff.isFinished)
            {
                buffList.Add(buff);
                continue;
            }
            
            buff.Tick(Time.deltaTime);
        }

        if (buffList.Count > 0)
        {
            while (buffList.Count > 0)
            {
                RemoveBuff(buffList[buffList.Count - 1]);
                buffList.RemoveAt(buffList.Count - 1);
            }
        }
    }

    public void AddBuff(Buff buff)
    {
        if (buff == null)
        {
            return;
        }
        
        if (buffs.ContainsKey(buff.buffData))
        {
            buffs[buff.buffData].Activate();
        }
        else
        {
            buffs.Add(buff.buffData, buff);
            buff.Activate();
            buff.ApplyEffect();
        }
    }
    
    public void RemoveBuff(Buff buff)
    {
        if (buff == null)
        {
            return;
        }
        
        if (buffs.ContainsKey(buff.buffData))
        {
            buff.End();
            buff.buffData.buff = null;
            buffs.Remove(buff.buffData);
        }
    }
}
