using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buffs/New Speed Percent Buff")]
public class SpeedBuffPercentScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override void InitializeBuff(GameObject obj)
    {
        buff = new SpeedBuffPercent(this, obj);
        
        if (obj.TryGetComponent(out BuffSystem buffs))
        {
            buffs.AddBuff(buff);
        }
    }
}