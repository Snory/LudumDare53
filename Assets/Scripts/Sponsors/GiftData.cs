using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GiftData : ScriptableObject
{
    public Sprite GiftSprite;

    public abstract void ApplyGift(Unit recipient);

}

