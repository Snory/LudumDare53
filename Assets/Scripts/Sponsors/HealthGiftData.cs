using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGift", menuName = "Gift/HealthGift")]

public class HealthGiftData : GiftData
{
    public int Health;

    public override void ApplyGift(Unit recipient)
    {
        recipient.AddHealth(Health);
    }
}

