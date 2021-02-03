using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    private readonly Dictionary<ScriptableObjectBuff, Buff> _buffs = new Dictionary<ScriptableObjectBuff, Buff>();

    void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        foreach (var buff in _buffs.Values)
        {
            if (buff.isPersist)
            {
                return;
            }
            
            buff.Tick(Time.deltaTime);
            if (buff.isFinished)
            {
                _buffs.Remove(buff.buff);
            }
        }
    }

    public void AddBuff(Buff buff)
    {
        if (_buffs.ContainsKey(buff.buff))
        {
            _buffs[buff.buff].Activate();
        }
        else
        {
            _buffs.Add(buff.buff, buff);
            buff.Activate();
        }
    }
}
