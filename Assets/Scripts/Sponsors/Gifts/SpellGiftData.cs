using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellGiftData", menuName = "Gift/NewSpellGiftData")]
public class SpellGiftData : GiftData
{
    public SpellData WeaponData;
    public override void ApplyGift(Unit recipient, bool correctRecipient)
    {

        if (correctRecipient)
        {
            recipient.AddScore(15);
        }
        else
        {
            recipient.AddScore(5);
        }

        recipient.SetSpell(WeaponData);
    }

    public override Sprite GetUnwrapedGiftSprite()
    {
        return WeaponData.SpellSprite;
    }
}
