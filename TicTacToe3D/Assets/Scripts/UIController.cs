using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverOverlay;

    [SerializeField] private Text _gameResult;

    private void Start()
    {
        var game = FindObjectOfType<Game>();
        game.OnGameFinished += FinishGame;
        game.OnGameReseted += ResetGame;
    }

    private void FinishGame(string result)
    {
        _gameResult.text = result;
        _gameOverOverlay.SetActive(true);
    }

    private void ResetGame()
    {
        _gameOverOverlay.SetActive(false);
    }
}
