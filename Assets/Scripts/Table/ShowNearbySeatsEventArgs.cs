using System;

internal class ShowNearbySeatsEventArgs : EventArgs
{
    public int Range;
    public Seat SeatRequestor;
    public bool Show;

    public ShowNearbySeatsEventArgs(Seat seat, int range, bool show)
    {
        this.SeatRequestor = seat;
        this.Range = range;
        Show = show;
    }
}