using UnityEngine;

public class ScoreManager : Singleton<ScoreManager> {

    [SerializeField] private TMPro.TextMeshProUGUI _scoreText = null;

    private int _score = 0;

    private void Start() {

        UpdateScoreText();
    }

    public void SetScore(int score) {

        _score = score;
        UpdateScoreText();
    }

    public void IncreaseScore(int amount) {

        _score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        _scoreText.text = "Score: " + _score;
    }
}
