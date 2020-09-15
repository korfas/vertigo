using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : Singleton<GameGrid> {

    // The necessary proportion to make hexagon equilateral = Sqrt(3) / 2
    private static float HEIGHT_TO_WIDTH_PROP = 0.866f;
    // Square root of 3
    private static float SQRT_3 = 1.732f;

    [SerializeField] private GameObject _gridObject = null;
    [SerializeField] private GameObject _hexagonsParent = null;
    [SerializeField] private GameObject _hexagonCoordinatesParent = null;
    [SerializeField] private GameObject _buttonsParent = null;
    [SerializeField] private GameObject _hexagonPrefab = null;
    [SerializeField] private GameObject _hexagonCoordinatePrefab = null;
    [SerializeField] private GameObject _buttonPrefab = null;

    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int _hexagonPaddingPercent = 0;

    public event Action OnInitialized;
    public bool isInitialized { get; private set; } = false;

    public GameObject[,] generatedHexagons { get; private set; }

    private float _hexagonWidth = 0.0f;
    private float _hexagonHeight = 0.0f;

    public void Apply() {

        if (_gridSize.x < 3 || _gridSize.y < 3) {
            Debug.Log("Grid size must be minimum 3x3");

        } else {
            GenerateCommonItems(true);
        }
    }

    private void Start() {

        Init();

    }

    public void Init() {
        GenerateCommonItems(false);
        SetSelectionButtons();

        isInitialized = true;
        OnInitialized?.Invoke();
    }

    private void GenerateCommonItems(bool showHexagonCoordinates) {
        ClearCurrentGrid();
        SetHexagons(showHexagonCoordinates);
    }

    public Vector2Int GetGridSize() {
        return _gridSize;
    }

    private void ClearCurrentGrid() {

        int hexagonsCount = _hexagonsParent.transform.childCount;
        int hexagonCoordinatesCount = _hexagonCoordinatesParent.transform.childCount;
        int buttonsCount = _buttonsParent.transform.childCount;

        for (int i = 0; i < hexagonsCount; i++) {
            DestroyImmediate(_hexagonsParent.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < hexagonCoordinatesCount; i++) {
            DestroyImmediate(_hexagonCoordinatesParent.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < buttonsCount; i++) {
            DestroyImmediate(_buttonsParent.transform.GetChild(0).gameObject);
        }
    }

    private void SetHexagons(bool showCoordinates) {

        generatedHexagons = new GameObject[_gridSize.y, _gridSize.x];

        float gridWidth = _gridObject.GetComponent<RectTransform>().rect.width;
        float gridHeight = _gridObject.GetComponent<RectTransform>().rect.height;

        float gridPosX = _gridObject.GetComponent<RectTransform>().position.x;
        float gridPosY = _gridObject.GetComponent<RectTransform>().position.y;

        float semiSideLength = GetHexagonSemiSideLength(gridWidth); // Half of Hexagon's one side length

        _hexagonWidth = semiSideLength * 4;
        float hexagonPercent = (100.0f - _hexagonPaddingPercent) / 100.0f;
        _hexagonWidth *= hexagonPercent;
        _hexagonHeight = _hexagonWidth * HEIGHT_TO_WIDTH_PROP;

        for (int j = 0; j < _gridSize.y; j++) {

            float deltaYForRow = j * _hexagonHeight * -1.0f;

            for (int i = 0; i < _gridSize.x; i++) {

                float posX = gridPosX - gridWidth / 2.0f + (2 + 3 * i) * semiSideLength;
                float posY = deltaYForRow + gridPosY + gridHeight / 2.0f - (0.5f * _hexagonHeight);
                if (i % 2 == 1) {
                    posY -= semiSideLength * SQRT_3;
                }
                Vector3 pos = new Vector3(posX, posY, 0);

                GameObject hexagon = Instantiate(_hexagonPrefab, pos, Quaternion.identity, _hexagonsParent.transform);

                hexagon.GetComponent<RectTransform>().sizeDelta = new Vector2(_hexagonWidth, _hexagonHeight);
                hexagon.name = j + "_" + i;
                generatedHexagons[j, i] = hexagon;

                GameObject hexagonCoordinate = Instantiate(_hexagonCoordinatePrefab, pos, Quaternion.identity, _hexagonCoordinatesParent.transform);
                hexagonCoordinate.GetComponent<HexagonCoordinate>().coordinates = new Vector2Int(j, i);
                hexagonCoordinate.name = j + "_" + i;


                hexagonCoordinate.GetComponent<HexagonCoordinate>().attachedHexagon = hexagon;
                hexagon.GetComponent<Hexagon>().attachedCoordinate = hexagonCoordinate;

                hexagon.GetComponent<Hexagon>().SetCoordinateTextActive(showCoordinates);
            }
        }

    }

    private void SetSelectionButtons() {

        float hexagon00PosX = generatedHexagons[0, 0].GetComponent<RectTransform>().position.x;
        float hexagon01PosX = generatedHexagons[0, 1].GetComponent<RectTransform>().position.x;
        float hexagon02PosX = generatedHexagons[0, 2].GetComponent<RectTransform>().position.x;
        float hexagon00PosY = generatedHexagons[0, 0].GetComponent<RectTransform>().position.y;
        float hexagon10PosY = generatedHexagons[1, 0].GetComponent<RectTransform>().position.y;
        float hexagon20PosY = generatedHexagons[2, 0].GetComponent<RectTransform>().position.y;

        float button00PosX = (hexagon00PosX + hexagon01PosX) / 2.0f;
        float button00PosY = (hexagon00PosY + hexagon10PosY) / 2.0f; 

        float button01PosX = (hexagon01PosX + hexagon02PosX) / 2.0f; 
        float button10PosY = (hexagon10PosY + hexagon20PosY) / 2.0f;

        float distanceX = button01PosX - button00PosX;
        float distanceY = (button10PosY - button00PosY) / 2;

        float buttonSizeX = Mathf.Abs(distanceX);
        float buttonSizeY = Mathf.Abs(distanceY);

        for (int j = 0; j < (_gridSize.y - 1) * 2; j++) {

            for (int i = 0; i < _gridSize.x - 1; i++) {

                float posX = button00PosX + distanceX * i;
                float posY = button00PosY + distanceY * j;

                Vector3 pos = new Vector3(posX, posY, 0);

                GameObject button = Instantiate(_buttonPrefab, pos, Quaternion.identity, _buttonsParent.transform);
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonSizeX, buttonSizeY);
                button.GetComponent<GridButton>().coordinates = new Vector2Int(j, i);
                button.GetComponent<GridButton>().SetCoordinateText();
                button.name = j + "_" + i;
            }
        }

    }

    private float GetHexagonSemiSideLength(float gridWidth) {

        float numberOfHexagonSides = 0;
        for (int i = 0; i < _gridSize.x; i++) {
            numberOfHexagonSides += i % 2 == 0 ? 2.0f : 1.0f;
        }
        if (_gridSize.x % 2 == 0) {
            numberOfHexagonSides += 0.5f;
        }

        float hexagonSideLength = gridWidth / numberOfHexagonSides;
        return hexagonSideLength / 2.0f;
    }

    public void InstantiateNewHexagon(Vector3 pos) {
        GameObject hexagon = Instantiate(_hexagonPrefab, pos, Quaternion.identity, _hexagonsParent.transform);
        hexagon.GetComponent<RectTransform>().sizeDelta = new Vector2(_hexagonWidth, _hexagonHeight);
        hexagon.GetComponent<Hexagon>().Init();
    }

}
