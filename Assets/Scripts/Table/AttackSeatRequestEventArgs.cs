using System;

internal class AttackSeatRequestEventArgs : EventArgs
{
    public Unit RequestingUnit;
    public Seat AttackedSeat;
    public int AttackPower;
    public int AttackRange;
    public Action<bool> RequestAttackCallback;


    public AttackSeatRequestEventArgs(Unit unit, Seat attackedSeat, int range, int power, Action<bool> requestAttackCallback)
    {
        this.RequestingUnit = unit;
        this.AttackedSeat = attackedSeat;
        this.AttackRange = range;
        this.AttackPower = power;
        this.RequestAttackCallback = requestAttackCallback;
    }
}