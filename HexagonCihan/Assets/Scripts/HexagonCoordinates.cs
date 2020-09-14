using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCoordinates : Singleton<HexagonCoordinates> {

    private GameObject[] _allHexagonCoordinates;

    public event Action OnInitialized;
    public bool isInitialized { get; private set; } = false;

    private Vector2Int _gridSize;

    private void Start() {

        if (GameGrid.Instance.isInitialized) {
            Init();
        } else {
            GameGrid.Instance.OnInitialized += Init;
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

    public void Init() {

        _gridSize = GameGrid.Instance.GetGridSize();

        int hexagonCoordinatesCount = transform.childCount;
        _allHexagonCoordinates = new GameObject[hexagonCoordinatesCount];

        for (int i = 0; i < hexagonCoordinatesCount; i++) {
            _allHexagonCoordinates[i] = transform.GetChild(i).gameObject;

            GameObject attachedHexagon = _allHexagonCoordinates[i].GetComponent<HexagonCoordinate>().attachedHexagon;

            if (attachedHexagon != null) {
                attachedHexagon.GetComponent<Hexagon>().Init();
            }
        }

        isInitialized = true;
        OnInitialized?.Invoke();
    }

    public Vector3 GetCoordinatePosition(int x, int y) {
        return transform.Find(x + "_" + y).GetComponent<RectTransform>().position;
    }

    public GameObject Get(int x, int y) {

        return transform.Find(x + "_" + y).gameObject;
    }

    public GameObject GetHexagon(int x, int y) {

        return transform.Find(x + "_" + y).gameObject.GetComponent<HexagonCoordinate>().attachedHexagon;
    }

    public void DeactivateAllSelectors() {

        foreach (GameObject hexagonCoordinate in _allHexagonCoordinates) {
            if (hexagonCoordinate != null) {
                GameObject hexagon = hexagonCoordinate.GetComponent<HexagonCoordinate>().attachedHexagon;
                if (hexagon != null) {
                    hexagon.GetComponent<Hexagon>().SetSelectorActivate(false);
                }
            }
        }
    }

    public void FillGaps() {

        ContinueFillingGaps(_gridSize.y - 1, 0);

    }

    private void FillCompleted() {
        //StartCoroutine(StartCheck());
    }

    private IEnumerator StartCheck() {

        yield return new WaitForSeconds(0.5f);

        bool isMatch = GridButtons.Instance.CheckMatchAfterDestroy();
        if (!isMatch) {
            GridButtons.Instance.UpdateSurroundedHexagonColorIndexes();
        }
    }

    private void ContinueFillingGaps(int startX, int startY) {

        int jLoop = startY;

        for (int i = startX; i >= 0; i--) {

            bool willBreak = false;

            for (int j = jLoop; j < _gridSize.x; j++) {

                GameObject hexagonCoordinate = Get(i, j);
                bool isHexEmpty = hexagonCoordinate.GetComponent<HexagonCoordinate>().IsEmpty();

                if (isHexEmpty) {

                    CheckTop(hexagonCoordinate, i, j);
                    willBreak = true;
                    if (i == 0 && j == _gridSize.x - 1) {
                        FillCompleted();
                    }
                    break;
                }
                jLoop = 0;

                if (i == 0 && j == _gridSize.x - 1) {
                    FillCompleted();
                }
            }

            if (willBreak)
                break;
        }
    }

    private void CheckTop(GameObject hexagonCoordinate, int x, int y) {

        bool filled = false;

        if (x > 0) {
            for (int i = x - 1; i >= 0; i--) {

                bool isTopHexEmpty = Get(i, y).GetComponent<HexagonCoordinate>().IsEmpty();
                if (!isTopHexEmpty) {

                    SlideHexagonToCoordinate(hexagonCoordinate, GetHexagon(i, y));
                    filled = true;
                    break;
                }
            }
        }

        if (!filled) {
            Vector3 pos = hexagonCoordinate.GetComponent<RectTransform>().position;
            Vector2Int hexCoor = hexagonCoordinate.GetComponent<HexagonCoordinate>().coordinates;
            GameGrid.Instance.InstantiateNewHexagon(pos);
            ContinueFillingGaps(hexCoor.x, hexCoor.y + 1);

            /*for (int i = x; i >= 0; i--) {

                Vector3 pos = Get(i, y).GetComponent<RectTransform>().position;
                GameGrid.Instance.InstantiateNewHexagon(pos);

                Vector2Int hexCoor = hexagonCoordinate.GetComponent<HexagonCoordinate>().coordinates;
                //ContinueFillingGaps(hexCoor.x, hexCoor.y);
            }*/
        }
    }


    public void SlideHexagonToCoordinate(GameObject hexagonCoordinate, GameObject hexagon) {

        Vector3 newPos = hexagonCoordinate.GetComponent<RectTransform>().position;
        Vector2Int hexCoor = hexagonCoordinate.GetComponent<HexagonCoordinate>().coordinates;

        LeanTween.moveY(hexagon, newPos.y, 0.1f).setOnComplete(() => {


            ContinueFillingGaps(hexCoor.x, hexCoor.y);
            //if (coordinates.x == 0) {
            //    GameGrid.Instance.InstantiateNewHexagon(transform.position);
            //    Debug.Log("Instantiate 1");
            //}
            //if (coordinates.x == 1 && step == 2) {
            //    GameGrid.Instance.InstantiateNewHexagon(transform.position);
            //    Debug.Log("Instantiate 2");
            //}

        });

    }

    public void SlideBottom(int x, int y, int step) {

        int newX = x + step;
        GameObject hex = GetHexagon(x, y);

        Vector3 newPos = GetCoordinatePosition(newX, y);

        LeanTween.moveY(hex, newPos.y, 0.2f).setOnComplete(() => {
            
            if (x == 0) {
                GameGrid.Instance.InstantiateNewHexagon(GetCoordinatePosition(x,y));
                Debug.Log("Instantiate 1");
            }
            if (x == 1 && step == 2) {
                GameGrid.Instance.InstantiateNewHexagon(GetCoordinatePosition(x, y));
                Debug.Log("Instantiate 2");

                StartCoroutine(StartCheck());
            }

        });


    }
}