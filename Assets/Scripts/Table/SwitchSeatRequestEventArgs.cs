using System;

public class SwitchSeatRequestEventArgs : EventArgs
{
    public Unit RequestingUnit;
    public Seat RequestedSeat;
    public Action<bool> SeatSwitchedCallback;

    public SwitchSeatRequestEventArgs(Unit requestingUnit, Seat requestedSeat, Action<bool> seatSwitchedCallback)
    {
        RequestingUnit = requestingUnit;
        RequestedSeat = requestedSeat;
        SeatSwitchedCallback = seatSwitchedCallback;
    }
}
