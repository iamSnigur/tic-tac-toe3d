using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private CellState[,] _board;

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    [SerializeField] private Transform _ground;

    [SerializeField] private GameObject _xPrefab;
    [SerializeField] private GameObject _oPrefab;

    private List<GameObject> _tokens;

    public int MarkedCells { get; private set; }

    public void InitializeBoard(Vector2Int boardSize)
    {
        Rows = boardSize.y;
        Columns = boardSize.x;
        _ground.localScale = new Vector3(boardSize.x, boardSize.y, 1f);
        _tokens = new List<GameObject>();
        _board = new CellState[Rows, Columns];
        MarkedCells = 0;
    }

    public CellState GetCellState(int row, int column) => _board[row, column];

    public bool SetCellState(int row, int column, CellState state, bool setToken)
    {
        if (column >= 0 && column < Columns && row >= 0 && row < Rows)
        {
            if (_board[row, column] != CellState.EMPTY && state != CellState.EMPTY)
            {
                return false;
            }

            if (setToken)
            {
                var offset = new Vector2((Columns - 1) * 0.5f, (Rows - 1) * 0.5f);

                var token = Instantiate((state == CellState.X) ? _xPrefab : _oPrefab);
                token.transform.SetParent(transform, false);
                token.transform.localPosition = new Vector3(column - offset.x, 0f, row - offset.y);

                _tokens.Add(token);
            }

            _board[row, column] = state;
            MarkedCells += (state == CellState.EMPTY) ? -1 : 1;
            return true;
        }

        return false;
    }
   
    public GameResult CheckState()
    {
        var tokensInARow = 0;

        var token = CellState.EMPTY;

        for (var x = 0; x < Columns; x++)
        {
            for(var y = 0; y < Rows - 1; y++)
            {
                if(_board[y, x] != CellState.EMPTY && _board[y, x] == _board[y + 1, x])
                {
                    tokensInARow++;
                    token = _board[y, x];
                }
                else
                {
                    break;
                }
                
            }

            if (tokensInARow == Rows - 1)
            {
                return token.WhoShouldWin();
            }
        }

        tokensInARow = 0;

        for (int y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns - 1; x++)
            {
                if (_board[y, x] != CellState.EMPTY && _board[y, x] == _board[y, x + 1])
                {
                    tokensInARow++;
                    token = _board[y, x];
                }
                else
                {
                    break;
                }
            }

            if (tokensInARow == Rows - 1)
            {
                return token.WhoShouldWin();
            }
        }

        tokensInARow = 0;

        for (int y = 0; y < Rows - 1; y++)
        {
            if (_board[y, y] != CellState.EMPTY && _board[y, y] == _board[y + 1, y + 1])
            {
                tokensInARow++;
                token = _board[y, y];
            }
            else
            {
                break;
            }

            if (tokensInARow == Rows - 1)
            {
                return token.WhoShouldWin();
            }
        }

        tokensInARow = 0;

        for (int y = 0; y < Rows - 1; y++)
        {
            if (_board[y, Rows - 1 - y] != CellState.EMPTY && _board[y, Rows - 1 - y] == _board[y + 1, Rows - 2 - y])
            {
                tokensInARow++;
                token = _board[y, Rows - 1 - y];
            }
            else
            {
                break;
            }

            if (tokensInARow == Rows - 1)
            {
                return token.WhoShouldWin();
            }
        }

        if (MarkedCells == Rows * Columns)
        {
            return GameResult.DRAW;
        }

        return GameResult.UNKNOWN;
    }

    public void ResetBoard()
    {
        _tokens.ForEach(t => Destroy(t));

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                _board[i, j] = CellState.EMPTY;
            }
        }
    }
}