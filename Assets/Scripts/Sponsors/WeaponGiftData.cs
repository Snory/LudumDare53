using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponGiftData", menuName = "Gift/NewWeaponGiftData")]
public class WeaponGiftData : GiftData
{
    public SpellData WeaponData;
    public override void ApplyGift(Unit recipient)
    {
        recipient.SetSpell(WeaponData);
    }
}
