using System;

public class ScoreboardModel
{
    private int _score;
    
    public int Score
    {
        get => _score;
        set
        {
            if (_score == value) return;
            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }

    public event Action<int> OnScoreChanged;

    public void AddScore(int points)
    {
        Score += points;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
