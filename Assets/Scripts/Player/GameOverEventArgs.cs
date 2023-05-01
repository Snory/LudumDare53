using System;

internal class GameOverEventArgs : EventArgs
{
    public float Score;

    public GameOverEventArgs(float score)
    {
        this.Score = score;
    }
}