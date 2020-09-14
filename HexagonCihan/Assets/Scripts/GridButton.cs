using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour {

    private enum MatchType { M_01, M_12, M_23, M_34, M_45, M_56, M_67, M_78, M_80 }

    [SerializeField] private GameObject _coordinateText = null;
    [SerializeField] private bool _debugCoordinates = false;

    //private Vector2Int hexagonVector1;
    //private Vector2Int hexagonVector2;
    //private Vector2Int hexagonVector3;

    public Vector2Int coordinates;

    public bool selected = false;
    private bool _rotating = false;
    private bool _isRightArrow = false;
    private bool _isTherePossibleMatch = false;

    //private GameObject[] _hexagons = new GameObject[3];
    //private GameObject[] _surroundedHexagons = new GameObject[9];
    private GameObject[] _hexagonCoordinates = new GameObject[3];
    private GameObject[] _surroundedHexagonCoordinates = new GameObject[9];
    private int[] _surroundedHexagonColorIndexes = new int[9];

    private Dictionary<int, int> _possibleMatches;

    private void Start() {

        if (GridButtons.Instance.isInitCompleted) {
            Init();
        } else {
            GridButtons.Instance.InitComplete += Init;
        }

        InputManager.Instance.OnRotated += HandleRotated;
    }

    private void OnDestroy() {

        try {
            GridButtons.Instance.InitComplete -= Init;
            InputManager.Instance.OnRotated -= HandleRotated;

        } catch (Exception e) {
            //Debug.Log("Could not unregister callback: " + e.Message);
        }

    }

    private void Init() {

        //SetHexagons();
        SetSurroundedHexagons();
        CheckIsTherePossibleMatch();

        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    public void SetHexagons() {

        int cx = coordinates.x;
        int cy = coordinates.y;

        int hexagon01Y = (cx + cy) % 2 == 0 ? cy : cy + 1;
        int hexagon2Y = (cx + cy) % 2 == 0 ? cy + 1 : cy;
        int hexagon2X = cx % 2 == 0 ? cx / 2 : cx / 2 + 1;

        _hexagonCoordinates[0] = HexagonCoordinates.Instance.Get(cx / 2, hexagon01Y);
        _hexagonCoordinates[1] = HexagonCoordinates.Instance.Get(cx / 2 + 1, hexagon01Y);
        _hexagonCoordinates[2] = HexagonCoordinates.Instance.Get(hexagon2X, hexagon2Y);

        //_hexagons[0] = Hexagons.Instance.Get(cx / 2, hexagon01Y);
        //_hexagons[1] = Hexagons.Instance.Get(cx / 2 + 1, hexagon01Y);
        //_hexagons[2] = Hexagons.Instance.Get(hexagon2X, hexagon2Y);

        //_hexagons[0].GetComponent<Hexagon>().attachedGridButton = gameObject;
        //_hexagons[1].GetComponent<Hexagon>().attachedGridButton = gameObject;
        //_hexagons[2].GetComponent<Hexagon>().attachedGridButton = gameObject;

        //_isRightArrow = _hexagons[2].transform.position.x > _hexagons[0].transform.position.x;
        _isRightArrow = _hexagonCoordinates[2].transform.position.x > _hexagonCoordinates[0].transform.position.x;
    }

    private void SetSurroundedHexagons() {

        //_surroundedHexagons = new List<GameObject>();
        //_surroundedHexagonColorIndexes = new List<int>();

        List<Vector2Int> surroundedHexagonIndexes = new List<Vector2Int>();
        Vector2Int gridSize = GameGrid.Instance.GetGridSize();

        //int hx = _hexagons[0].GetComponent<Hexagon>().coordinates.x;
        //int hy = _hexagons[0].GetComponent<Hexagon>().coordinates.y;
        int hx = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().coordinates.x;
        int hy = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().coordinates.y;

        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx - 1 : hx, hy - 1));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx - 1 : hx - 1, hy));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx - 1 : hx, hy + 1));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx : _isRightArrow ? hx : hx + 1, _isRightArrow ? hy + 2 : hy + 1));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx + 1 : _isRightArrow ? hx + 1 : hx + 2, _isRightArrow ? hy + 2 : hy + 1));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? _isRightArrow ? hx + 1 : hx + 2 : hx + 2, _isRightArrow ? hy + 1 : hy));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? _isRightArrow ? hx + 2 : hx + 1 : hx + 2, _isRightArrow ? hy : hy - 1));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx + 1 : _isRightArrow ? hx + 2 : hx + 1, _isRightArrow ? hy - 1 : hy - 2));
        surroundedHexagonIndexes.Add(new Vector2Int(hy % 2 == 0 ? hx : _isRightArrow ? hx + 1 : hx, _isRightArrow ? hy - 1 : hy - 2));

        for (int i = 0; i < surroundedHexagonIndexes.Count; i++) {

            Vector2Int vctr = surroundedHexagonIndexes[i];

            if (vctr.x >= 0 && vctr.y >= 0 && vctr.x < gridSize.y && vctr.y < gridSize.x) {
                //GameObject hexagon = Hexagons.Instance.Get(vctr.x, vctr.y);
                //_surroundedHexagons[i] = hexagon;
                //_surroundedHexagonColorIndexes[i] = hexagon.GetComponent<Hexagon>().colorIndex;
                GameObject hexagonCoordinate = HexagonCoordinates.Instance.Get(vctr.x, vctr.y);
                _surroundedHexagonCoordinates[i] = hexagonCoordinate;
                _surroundedHexagonColorIndexes[i] = hexagonCoordinate.GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex;

            } else {
                _surroundedHexagonCoordinates[i] = null;
                _surroundedHexagonColorIndexes[i] = -1;
            }
        }
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            RotateSequently(false);
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            RotateSequently(true);
        }
    }

    private void HandleRotated(string direction) {

        if (direction == "ccw") {
            RotateSequently(false);
        }
        if (direction == "cw") {
            RotateSequently(true);
        }
    }


    private void RotateSequently(bool isRight) {
        if (selected && !_rotating) {

            if (_isTherePossibleMatch) {

                Rotate(isRight, () => {

                    bool isMatch1 = GridButtons.Instance.CheckMatch();
                    if (!isMatch1) {

                        Rotate(isRight, () => {

                            bool isMatch2 = GridButtons.Instance.CheckMatch();

                            if (!isMatch2) {
                                Rotate(isRight, null);
                            }

                        });
                    }

                });


            } else {
                RotateNotPossibleMatch(isRight);
            }
        }
    }

    private void RotateNotPossibleMatch(bool isRight) {
        Rotate(isRight, () => {
            Rotate(isRight, () => {
                Rotate(isRight, null);
            });
        });
    }

    private void Rotate(bool isRight, Action onComplete) {

        _rotating = true;

        Vector3 hexCoor0Pos = _hexagonCoordinates[0].GetComponent<RectTransform>().position;
        Vector3 hexCoor1Pos = _hexagonCoordinates[1].GetComponent<RectTransform>().position;
        Vector3 hexCoor2Pos = _hexagonCoordinates[2].GetComponent<RectTransform>().position;

        GameObject hex0 = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().attachedHexagon;
        GameObject hex1 = _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().attachedHexagon;
        GameObject hex2 = _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon;

        //Vector3 hex0Pos = hex0.GetComponent<RectTransform>().position;
        //Vector3 hex1Pos = hex1.GetComponent<RectTransform>().position;
        //Vector3 hex2Pos = hex2.GetComponent<RectTransform>().position;

        bool cond = (isRight && _isRightArrow) || (!isRight && !_isRightArrow);

        //Vector2 hex0NewPos = cond ? hex2Pos : hex1Pos;
        //Vector2 hex1NewPos = cond ? hex0Pos : hex2Pos;
        //Vector2 hex2NewPos = cond ? hex1Pos : hex0Pos;

        Vector2 hex0NewPos = cond ? hexCoor2Pos : hexCoor1Pos;
        Vector2 hex1NewPos = cond ? hexCoor0Pos : hexCoor2Pos;
        Vector2 hex2NewPos = cond ? hexCoor1Pos : hexCoor0Pos;

        LeanTween.move(hex0, hex0NewPos, 0.2f);
        LeanTween.move(hex1, hex1NewPos, 0.2f);
        LeanTween.move(hex2, hex2NewPos, 0.2f).setOnComplete(() => {
            _rotating = false;
            onComplete?.Invoke();
        });

        //_hexagons[0] = cond ? hex1 : hex2;
        //_hexagons[1] = cond ? hex2 : hex0;
        //_hexagons[2] = cond ? hex0 : hex1;


    }

    public bool CheckIsMatchingOnStart() {

        if (IsMatching()) {
            //_hexagons[2].GetComponent<Hexagon>().ChangeColorToNext();
            _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().ChangeColorToNext();
            return true;
        }
        return false;
    }

    public bool IsMatching() {

        //int hex0Color = _hexagons[0].GetComponent<Hexagon>().colorIndex;
        //int hex1Color = _hexagons[1].GetComponent<Hexagon>().colorIndex;
        //int hex2Color = _hexagons[2].GetComponent<Hexagon>().colorIndex;

        int hex0Color = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex;
        int hex1Color = _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex;
        int hex2Color = _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex;

        return hex0Color == hex1Color && hex0Color == hex2Color;

    }

    public bool CheckMatch() {

        GameObject hex0 = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().attachedHexagon;
        GameObject hex1 = _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().attachedHexagon;
        GameObject hex2 = _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon;

        if (hex0 != null && hex1 != null && hex2 != null) {
            int hc0 = hex0.GetComponent<Hexagon>().colorIndex;
            int hc1 = hex1.GetComponent<Hexagon>().colorIndex;
            int hc2 = hex2.GetComponent<Hexagon>().colorIndex;

            if (hc0 == hc1 && hc0 == hc2) {

                DestroyMatched(hex0, hex1, hex2);
                return true;
            }
        }

        return false;
    }

    private void DestroyMatched(GameObject hex0, GameObject hex1, GameObject hex2) {

        Destroy(hex0);
        Destroy(hex1);
        Destroy(hex2);

        _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().attachedHexagon = null;
        _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().attachedHexagon = null;
        _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon = null;

        ScoreManager.Instance.IncreaseScore(15);

        HexagonCoordinates.Instance.FillGaps();
        /*
        for (int i = destroyedHexVectors[0].x - 1; i >= 0; i--) {
            HexagonCoordinates.Instance.Get(i, destroyedHexVectors[0].y).GetComponent<HexagonCoordinate>().SlideBottom(1);
        }

        for (int i = destroyedHexVectors[1].x - 1; i >= 0; i--) {
            HexagonCoordinates.Instance.Get(i, destroyedHexVectors[1].y).GetComponent<HexagonCoordinate>().SlideBottom(2);
        }
        */
    }

    public bool CheckIsTherePossibleMatch() {

        _possibleMatches = new Dictionary<int, int>();

        for (int i = 0; i < 9; i++) {

            int nextI = (i + 1) % 9;

            int currentColorIndex = _surroundedHexagonColorIndexes[i];
            int nextColorIndex = _surroundedHexagonColorIndexes[nextI];

            if (currentColorIndex >= 0 && nextColorIndex >= 0 && currentColorIndex == nextColorIndex) {

                if (IsThereHexagonWithColorIndex(currentColorIndex)) {

                    _possibleMatches.Add(i, currentColorIndex);
                    _isTherePossibleMatch = true;
                }
            }
        }

        return _isTherePossibleMatch;

    }




    private bool IsThereHexagonWithColorIndex(int colorIndex) {

        //return _hexagons[0].GetComponent<Hexagon>().colorIndex == colorIndex ||
        //       _hexagons[1].GetComponent<Hexagon>().colorIndex == colorIndex ||
        //       _hexagons[2].GetComponent<Hexagon>().colorIndex == colorIndex;

        bool is0Empty = _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().IsEmpty();
        bool is1Empty = _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().IsEmpty();
        bool is2Empty = _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().IsEmpty();

        if (!is0Empty && !is1Empty && !is2Empty) {
            return _hexagonCoordinates[0].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex == colorIndex ||
                   _hexagonCoordinates[1].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex == colorIndex ||
                   _hexagonCoordinates[2].GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().colorIndex == colorIndex;
        } else {
            return false;
        }
    }

    private void HandleClick() {

        GridButtons.Instance.DeselectAll();
        //Hexagon.Instance.DeactivateAllSelectors();
        HexagonCoordinates.Instance.DeactivateAllSelectors();

        selected = true;

        //_hexagons[0]?.GetComponent<Hexagon>().SetSelectorActivate(true);
        //_hexagons[1]?.GetComponent<Hexagon>().SetSelectorActivate(true);
        //_hexagons[2]?.GetComponent<Hexagon>().SetSelectorActivate(true);

        _hexagonCoordinates[0]?.GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().SetSelectorActivate(true);
        _hexagonCoordinates[1]?.GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().SetSelectorActivate(true);
        _hexagonCoordinates[2]?.GetComponent<HexagonCoordinate>().attachedHexagon.GetComponent<Hexagon>().SetSelectorActivate(true);

        //foreach (GameObject hexagon in _surroundedHexagons) {
        //    hexagon.GetComponent<Hexagon>().SetSelectorActivate(true);
        //}
    }




    public void SetCoordinateText() {

        if (_debugCoordinates) {
            _coordinateText.SetActive(true);
            _coordinateText.GetComponent<TMPro.TextMeshProUGUI>().text = "(" + coordinates.x + ", " + coordinates.y + ")";

        } else {
            _coordinateText.SetActive(false);
        }
    }
}
