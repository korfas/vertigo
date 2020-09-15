public class GameManager : Singleton<GameManager> {

    public enum GameState { PLAYING, GAME_OVER }

    public GameState gameState { get; private set; } = GameState.PLAYING;

    public void UpdateState(GameState state) {

        gameState = state;

        switch (gameState) {
            case GameState.PLAYING:
                GameOverMenu.Instance.Show(false);
                GameGrid.Instance.Init();
                ScoreManager.Instance.SetScore(0);
                break;

            case GameState.GAME_OVER:
                GameOverMenu.Instance.Show(true);
                break;
        }
    }
}
