using System;
using UnityEngine;

public class GridButtons : Singleton<GridButtons> {

    private GameObject[] _allGridButtons = null;

    public event Action InitComplete;
    public bool isInitCompleted { get; private set; } = false;

    private void Start() {

        if (HexagonCoordinates.Instance.isInitialized) {
            Init();
        } else {
            HexagonCoordinates.Instance.OnInitialized += Init;
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        try {
            GameGrid.Instance.OnInitialized -= Init;

        } catch (Exception) { }
    }

    private void Init() {

        int gridButtonsCount = transform.childCount;
        _allGridButtons = new GameObject[gridButtonsCount];

        for (int i = 0; i < gridButtonsCount; i++) {
            _allGridButtons[i] = transform.GetChild(i).gameObject;
            _allGridButtons[i].GetComponent<GridButton>().SetHexagons();
        }

        CheckIsMatchingOnStart();
    }

    public void DeselectAll() {

        foreach (GameObject gridButton in _allGridButtons) {
            gridButton.GetComponent<GridButton>().selected = false;
        }
    }

    public bool CheckMatchAfterDestroy() {

        bool isMatch = false;

        foreach (GameObject gridButton in _allGridButtons) {
            if (gridButton.GetComponent<GridButton>().CheckMatchAfterDestroy()) {
                isMatch = true;
            }
        }
        return isMatch;
    }

    public void UpdateSurroundedHexagonColorIndexes() {
        foreach (GameObject gridButton in _allGridButtons) {
            gridButton.GetComponent<GridButton>().UpdateSurroundedHexagonColorIndexes();

        }
    }

    public void CheckIsMatchingOnStart() {

        bool isMatching = false;

        foreach (GameObject gridButton in _allGridButtons) {
            bool isMatchingGridButton = gridButton.GetComponent<GridButton>().CheckIsMatchingOnStart();
            if (isMatchingGridButton) {
                isMatching = true;
            }
        }

        if (isMatching) {
            CheckIsMatchingOnStart();
        } else {
            isInitCompleted = true;
            InitComplete?.Invoke();
        }
    }

    public void CheckIsTherePossibleMatches() {

        bool isTherePossibleMatch = false;
        foreach (GameObject gridButton in _allGridButtons) {
            bool isMatch = gridButton.GetComponent<GridButton>().CheckIsTherePossibleMatch();
            if (isMatch) {
                isTherePossibleMatch = true;
            }
        }

        if (!isTherePossibleMatch) {
            GameManager.Instance.UpdateState(GameManager.GameState.GAME_OVER);
        }
    }
}
