using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class DefendSeatRequestEventArgs : EventArgs
{
    public Unit RequestingUnit;
    public Seat RequestedSeat;
    public float DefendTime;
    public Action<bool> SeatDefendCallback;

    public DefendSeatRequestEventArgs(Unit requestingUnit, Seat requestedSeat, float defendTime, Action<bool> seatDefendCallback)
    {
        RequestingUnit = requestingUnit;
        RequestedSeat = requestedSeat;
        DefendTime = defendTime;
        SeatDefendCallback = seatDefendCallback;
    }
}
