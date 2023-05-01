using System;

public class GiftQueueEventArgs : EventArgs
{
    public GiftData Data;

    public GiftQueueEventArgs(GiftData data)
    {
        this.Data = data;
    }
}