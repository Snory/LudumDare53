using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGift", menuName = "Gift/CooldownGift")]
public class CooldownGiftData : GiftData
{
    public Sprite UnwrappedSprite;

    public override void ApplyGift(Unit recipient, bool correctRecipient)
    {
        if (correctRecipient)
        {
            recipient.AddScore(15);
        } else
        {
            recipient.AddScore(5);
        }

        recipient.ResetCoolDown();
    }

    public override Sprite GetUnwrapedGiftSprite()
    {
        return UnwrappedSprite;
    }
}
