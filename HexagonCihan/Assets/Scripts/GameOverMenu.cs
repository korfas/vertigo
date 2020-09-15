using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : Singleton<GameOverMenu> {

    [SerializeField] private GameObject _gameOverMenuInner = null;
    [SerializeField] private Button _restartBtn = null;

    private void Start() {

        _restartBtn.onClick.AddListener(HandleRestartClicked);
    }

    private void HandleRestartClicked() {

        GameManager.Instance.UpdateState(GameManager.GameState.PLAYING);
    }

    public void Show(bool show) {
        _gameOverMenuInner.SetActive(show);
    }
}
