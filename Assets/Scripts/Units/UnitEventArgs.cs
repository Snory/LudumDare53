using System;

internal class UnitEventArgs : EventArgs
{
    public Unit Unit;

    public UnitEventArgs(Unit unit)
    {
        this.Unit = unit;
    }
}