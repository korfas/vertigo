using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCoordinates : Singleton<HexagonCoordinates> {

    private GameObject[] _allHexagonCoordinates;

    public event Action OnInitialized;
    public bool isInitialized { get; private set; } = false;

    private List<Vector2Int> _coordinatesToInstantiateAbove;
    private List<Vector2Int> _coordinatesToFill;
    private List<GameObject> _hexagonsToSlide;
    private List<float> _targetPositions;

    private int _numberOfCurrentlySlidingHexagons = 0;

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

        } catch (Exception) { }
    }

    public void Init() {

        InitLists();

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

    private void InitLists() {

        _coordinatesToInstantiateAbove = new List<Vector2Int>();
        _coordinatesToFill = new List<Vector2Int>();
        _hexagonsToSlide = new List<GameObject>();
        _targetPositions = new List<float>();
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

    private IEnumerator StartCheck() {

        yield return new WaitForSeconds(0.5f);

        bool isMatch = GridButtons.Instance.CheckMatchAfterDestroy();
        if (!isMatch) {
            GridButtons.Instance.UpdateSurroundedHexagonColorIndexes();
            
        }
    }


    public void AddCoordinateToFill(Vector2Int coordinate) {

        bool addCoordinate = true;

        for (int i = 0; i < _coordinatesToFill.Count; i++) {
            if (_coordinatesToFill[i].y == coordinate.y) {
                if (_coordinatesToFill[i].x >= coordinate.x) {
                    addCoordinate = false;
                    break;
                } else {
                    _coordinatesToFill.RemoveAt(i);
                }
            }
        }
        if (addCoordinate) {
            _coordinatesToFill.Add(coordinate);
        }
    }

    public void StartFilling() {

        foreach (Vector2Int coordinate in _coordinatesToFill) {
            FillCoordinatesOfColumn(coordinate.x, coordinate.y);
        }
    }

    public void FillCoordinatesOfColumn(int cx, int cy) {

        int numberOfFilledGap = 0;

        for (int i = cx - 1; i >= 0; i--) {
            GameObject hexagon = GetHexagon(i, cy);
            if (hexagon != null) {
                _hexagonsToSlide.Add(hexagon);
                _targetPositions.Add(GetCoordinatePosition(cx - numberOfFilledGap, cy).y);
                //_coordinatesToSlideAndTargetPositions.Add(new Vector2Int(i, cy), GetCoordinatePosition(cx - numberOfFilledGap, cy).y);
                numberOfFilledGap++;
            }
        }
        _coordinatesToInstantiateAbove.Add(new Vector2Int(cx - numberOfFilledGap, cy));

        StartSliding();

    }

    private void StartSliding() {

        for (int i = 0; i < _hexagonsToSlide.Count; i++) {

            SlideToCoordinate(_hexagonsToSlide[i], _targetPositions[i]);
        }
    }

    private void InstantiateHexagonToCoordinateAndAbove() {

        foreach (Vector2Int coordinate in _coordinatesToInstantiateAbove) {

            int cx = coordinate.x;
            int cy = coordinate.y;

            for (int i = cx; i >= 0; i--) {
                GameGrid.Instance.InstantiateNewHexagon(GetCoordinatePosition(i, cy));
            }
        }
    }

    public void SlideToCoordinate(GameObject hexagon, float y) {

        _numberOfCurrentlySlidingHexagons++;

        LeanTween.moveY(hexagon, y, 0.2f).setOnComplete(() => {

            _numberOfCurrentlySlidingHexagons--;

            if (_numberOfCurrentlySlidingHexagons < 1) {
                _numberOfCurrentlySlidingHexagons = 0;

                InstantiateHexagonToCoordinateAndAbove();
                InitLists();
                StartCoroutine(StartCheck());
            }
        });

    }


}