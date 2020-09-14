using System;
using UnityEngine;

public class GridButtons : Singleton<GridButtons> {

    private GameObject[] _allGridButtons = null;

    public event Action InitComplete;
    public bool isInitCompleted { get; private set; } = false;

    private void Start() {

        //if (Hexagons.Instance.isInitialized) {
        //    Init();
        //} else {
        //    Hexagons.Instance.OnInitialized += Init;
        //}

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

        } catch (Exception e) {
            //Debug.Log("Could not unregister callback: " + e.Message);
        }
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
    /*
    public void CheckMatchesAfterDestroy() {

        foreach (GameObject gridButton in _allGridButtons) {
            gridButton.GetComponent<GridButton>().CheckMatch();
        }
        foreach (GameObject gridButton in _allGridButtons) {
            gridButton.GetComponent<GridButton>().CheckIsTherePossibleMatch();   
        }

    }
    */

    public bool CheckMatch() {

        bool isMatch = false;

        foreach (GameObject gridButton in _allGridButtons) {
            if (gridButton.GetComponent<GridButton>().CheckMatch()) {
                isMatch = true;
            }
        }
        return isMatch;
    }

    public void CheckPossibleMatches() {

        foreach (GameObject gridButton in _allGridButtons) {
            gridButton.GetComponent<GridButton>().CheckIsTherePossibleMatch();
             
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
}
