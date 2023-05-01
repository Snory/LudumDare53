using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twist : MonoBehaviour
{
    protected Seat _seat;

    public void SetSeat(Seat seat)
    {
        _seat = seat;
        _seat.SetUnderAttack(true);
    }

    private void OnSeatEmtpy(Seat s)
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public void OnTableReorganized()
    {
        this.transform.position = _seat.transform.position;
    }

    public void OnSeatRemoved(EventArgs args)
    {
        SeatEventArgs seatEventArgs = args as SeatEventArgs;

        if(seatEventArgs.Seat == _seat)
        {
            Destroy(this.gameObject);
        }
    }
}
