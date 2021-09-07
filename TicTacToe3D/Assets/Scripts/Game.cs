using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public event Action<string> OnGameFinished;
    public event Action OnGameReseted;

    [SerializeField] private GameBoard _board;

    [SerializeField] private Camera _camera;

    private CellState _currentPlayer;

    private bool _isPlayerSelected;
    private bool _isGameOver;

    private AI _ai;

    private Ray _touchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private Vector2Int _boardSize;

    private void Update()
    {     
        if(_isPlayerSelected && !_isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (HandleTouch())
                {
                    (int, int) move = _ai.DoBestMove(_currentPlayer.SwitchPlayer(), _currentPlayer.SwitchPlayer().WhoShouldWin());
                    _board.SetCellState(move.Item1, move.Item2, _currentPlayer.SwitchPlayer(), true);
                }
            }

            if (_board.CheckState().HasEnded())
            {
                _isGameOver = true;
                OnGameFinished?.Invoke($"{_board.CheckState()}");
            }
        }        
    }

    private void InitializeGame()
    {
        _board.InitializeBoard(_boardSize);
        _ai = new AI(_board);
    }

    private bool HandleTouch()
    {
        RaycastHit hit;

        if (Physics.Raycast(_touchRay, out hit))
        {
            int x = (int)(hit.point.x + _board.Columns * 0.5f);
            int y = (int)(hit.point.z + _board.Rows * 0.5f);

            if (x >= 0 && x < _board.Columns && y >= 0 && y < _board.Rows)
            {
                return _board.SetCellState(y, x, _currentPlayer, true);
            }
        }

        return false;
    }

    // Shitty code #1
    public void SetPlayerTokenX()
    {
        _currentPlayer = CellState.X;
        _isPlayerSelected = true;
    }

    // Shitty code #2
    public void SetPlayerTokenO()
    {
        _currentPlayer = CellState.O;
        _isPlayerSelected = true;
    }

    public void ResetGame()
    {
        _isGameOver = false;
        _board.ResetBoard();
        OnGameReseted?.Invoke();
    }

    public void SetDificulty(int rows)
    {
        _boardSize = new Vector2Int(rows, rows);
        InitializeGame();
    }
}