
public class GiftQueued
{
    private GiftQueueItemUI _giftQueueItemUI;
    private GiftData _giftData;

    public GiftQueued(GiftData giftData, GiftQueueItemUI giftQueueItemUI = null)
    {
        _giftQueueItemUI = giftQueueItemUI;
        _giftData = giftData;
    }

    public GiftData GetPreparedGiftData()
    {
        return _giftData;
    }

    public GiftQueueItemUI GetPreparedQueueItemUI()
    {
        return _giftQueueItemUI;
    }
}
