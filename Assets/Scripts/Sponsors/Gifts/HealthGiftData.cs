using UnityEngine;

[CreateAssetMenu(fileName = "NewGift", menuName = "Gift/HealthGift")]

public class HealthGiftData : GiftData
{
    public int Health;
    public Sprite UnwrappedSprite;

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
        recipient.AddHealth(Health);
    }

    public override Sprite GetUnwrapedGiftSprite()
    {
        return UnwrappedSprite;
    }
}
