using System;

public class ScoreboardController : IDisposable
{
    private readonly ScoreboardModel _model;
    private readonly ScoreboardView _view;
    private readonly GameController _gameController;

    public ScoreboardController(ScoreboardModel model, ScoreboardView view, GameController gameController)
    {
        _model = model;
        _view = view;
        _gameController = gameController;

        _model.OnScoreChanged += HandleScoreChanged;
        if (_gameController != null)
        {
            _gameController.OnKillCountChanged += HandleKill;
        }
        
        // Initial setup
        _view.UpdateScore(_model.Score);
    }

    private void HandleScoreChanged(int newScore)
    {
        _view.UpdateScore(newScore);
    }

    private void HandleKill(int count)
    {
        _model.Score = count;
    }

    public void Dispose()
    {
        _model.OnScoreChanged -= HandleScoreChanged;
        if (_gameController != null)
        {
            _gameController.OnKillCountChanged -= HandleKill;
        }
    }
}
