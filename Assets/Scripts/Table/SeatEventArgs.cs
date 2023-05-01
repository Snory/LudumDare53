using System;

public class SeatEventArgs : EventArgs
{
    public Seat Seat;

    public SeatEventArgs(Seat seat)
    {
        this.Seat = seat;
    }
}