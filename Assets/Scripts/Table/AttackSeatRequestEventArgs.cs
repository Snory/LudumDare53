using System;

internal class AttackSeatRequestEventArgs : EventArgs
{
    public Unit RequestingUnit;
    public Seat AttackedSeat;
    public int AttackRange;
    public Action<bool> RequestAttackCallback;


    public AttackSeatRequestEventArgs(Unit unit, Seat attackedSeat, int range, Action<bool> requestAttackCallback)
    {
        this.RequestingUnit = unit;
        this.AttackedSeat = attackedSeat;
        this.AttackRange = range;
        this.RequestAttackCallback = requestAttackCallback;
    }
}