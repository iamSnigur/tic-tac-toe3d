public enum CellState
{
    EMPTY,
    X,
    O
}

public static class CellStateExtensions
{
    public static CellState SwitchPlayer(this CellState currentPlayer)
    {
        return currentPlayer == CellState.X ? CellState.O : CellState.X;
    }

    public static GameResult WhoShouldWin(this CellState currentPlayer)
    {
        return currentPlayer == CellState.X ? GameResult.XWINS : currentPlayer == CellState.O ? GameResult.OWINS : GameResult.UNKNOWN;
    }
}