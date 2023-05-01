using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GiftData : ScriptableObject
{
    public abstract void ApplyGift(Unit recipient, bool correctRecipient);

    public abstract Sprite GetUnwrapedGiftSprite();

}

