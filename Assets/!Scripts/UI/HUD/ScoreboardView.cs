using UnityEngine;
using TMPro;

public class ScoreboardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (_scoreText == null)
            _scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Kills: {score}";
        }
    }
}
