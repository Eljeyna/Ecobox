using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/New Weapon Item")]
public class ItemWeapon : Item
{
    public AssetReference idReferenceWeapon;
    private GameObject weapon;

    public override async void Use()
    {
        await WeaponChange();
    }

    public async Task WeaponChange()
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                if (Player.Instance.TryGetComponent(out NoWeapon noWeapon))
                {
                    if (weapon && weapon.TryGetComponent(out Gun gun))
                    {
                        Addressables.ReleaseInstance(weapon);
                        Player.Instance.weaponItem = null;
                        Player.Instance.weapon = noWeapon;
                        Player.Instance.weapon.enabled = true;

                        return;
                    }
                
                    weapon = await idReferenceWeapon.InstantiateAsync(Player.Instance.transform).Task;

                    if (weapon && weapon.TryGetComponent(out Gun gunA))
                    {
                        gunA.entity = Player.Instance;
                        Vector2 size = Player.Instance.thisCollider.size;
                        gunA.attackOffset = (size.x + size.y) / 2;

                        if (Player.Instance.weapon == noWeapon)
                        {
                            Player.Instance.weapon.enabled = false;
                        }
                        else
                        {
                            Addressables.ReleaseInstance(Player.Instance.weapon.gameObject);
                        }

                        Player.Instance.weaponItem = this;
                        Player.Instance.weapon = gunA;
                    }
                }

                break;
            default:
                return;
        }
    }

    private void OnDestroy()
    {
        if (weapon)
        {
            if (Player.Instance)
            {
                switch (itemType)
                {
                    case ItemType.Weapon:
                        if (weapon.TryGetComponent(out Gun gunA) && Player.Instance.weapon == gunA)
                        {
                            Player.Instance.weaponItem = null;
                            Player.Instance.weapon = null;
                        }

                        break;
                }
            }

            Addressables.ReleaseInstance(weapon);
        }
    }
}
