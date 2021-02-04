using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public readonly Dictionary<ScriptableObjectBuff, Buff> buffs = new Dictionary<ScriptableObjectBuff, Buff>();

    void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        foreach (var buff in buffs.Values)
        {
            if (buff.isPersist)
            {
                return;
            }
            
            buff.Tick(Time.deltaTime);
            if (buff.isFinished)
            {
                buffs.Remove(buff.buff);
            }
        }
    }

    public void AddBuff(Buff buff)
    {
        if (buffs.ContainsKey(buff.buff))
        {
            buffs[buff.buff].Activate();
        }
        else
        {
            buffs.Add(buff.buff, buff);
            buff.Activate();
        }
    }
}
