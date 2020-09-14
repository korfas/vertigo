using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> {

    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

    private int _score = 0;

    private void Start() {

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
