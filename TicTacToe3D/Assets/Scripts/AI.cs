using System;

public class AI
{
    private GameBoard _board;

    public AI(GameBoard board)
    {
        _board = board;
    }

    public (int, int) DoBestMove(CellState nextMovePlayer, GameResult targetResult)
    {
        var bestScore = -1000;

        (int, int) bestMove = (-1, -1);

        for (int i = 0; i < _board.Rows; i++)
        {
            for (int j = 0; j < _board.Columns; j++)
            {
                if (_board.GetCellState(i, j) == CellState.EMPTY)
                {
                    _board.SetCellState(i, j, nextMovePlayer, false);

                    // In my case Minimax algorithm does not work correctly for bigger boards than 3x3

                    //var tempScore = Minimax(0, -1000, 1000, nextMovePlayer.SwitchPlayer(), targetResult, true);
                    var tempScore = (int)UnityEngine.Random.Range(-10, 11);

                    _board.SetCellState(i, j, CellState.EMPTY, false);

                    if (tempScore > bestScore)
                    {
                        bestMove = (i, j);
                        bestScore = tempScore;
                    }
                }
            }
        }

        return bestMove;
    }

    private int Minimax(int depth, int alpha, int beta, CellState nextMovePlayer, GameResult targetResult, bool isMaximizing)
    {
        GameResult result = _board.CheckState();

        if (result.IsMyWin(targetResult))
        {
            return 10 - depth;
        }

        if (result.IsOponentWin(targetResult))
        {
            return -10 + depth;
        }

        if (result.HasEnded())
        {
            return 0;
        }

        var bestScore = isMaximizing ? -1000 : 1000;

        for (int i = 0; i < _board.Rows; i++)
        {
            for (int j = 0; j < _board.Columns; j++)
            {
                if (_board.GetCellState(i, j) == CellState.EMPTY)
                {
                    _board.SetCellState(i, j, nextMovePlayer, false);

                    var currentScore = Minimax(depth + 1, alpha, beta, nextMovePlayer.SwitchPlayer(), targetResult, !isMaximizing);

                    _board.SetCellState(i, j, CellState.EMPTY, false);

                    if (isMaximizing)
                    {
                        bestScore = Math.Max(bestScore, currentScore);
                        alpha = Math.Max(alpha, currentScore);
                    }
                    else
                    {
                        bestScore = Math.Min(bestScore, currentScore);
                        beta = Math.Min(beta, currentScore);
                    }

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
        }

        return bestScore;
    }
}