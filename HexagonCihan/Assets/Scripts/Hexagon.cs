using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour {

    [SerializeField] private GameObject _selector = null;
    [SerializeField] private GameObject _coordinateText = null;

    public int colorIndex { get; private set; } = 0;

    //public Vector2Int coordinates;
    public GameObject attachedCoordinate = null;

    //public GameObject attachedGridButton = null;

    private Colors _colors;

    private void Start() {


    }

    public void Init() {

        _selector.SetActive(false);

        GameObject designManager = GameObject.FindGameObjectWithTag("DesignManager");

        _colors = designManager.GetComponent<Colors>();

        int randomColorIndex = Random.Range(0, _colors.GetCount());
        colorIndex = randomColorIndex;

        SetUIColor();
        //SetCoordinateText();
    }
    /*
    public void SlideBottom(int step) {

        Vector2Int coordinates = attachedCoordinate.GetComponent<HexagonCoordinate>().coordinates;
        int newX = coordinates.x + step;

        Vector3 newPos = HexagonCoordinates.Instance.GetCoordinatePosition(newX, coordinates.y);

        LeanTween.moveY(gameObject, newPos.y, 0.2f);
    }
    */
    private void SetUIColor() {
        Color color = _colors.GetColor(colorIndex);
        GetComponent<Image>().color = color;
    }

    public void SetSelectorActivate(bool activate) {
        _selector.SetActive(activate);
    }

    public void ChangeColorToNext() {

        int nextColorIndex = (colorIndex + 1) % _colors.GetCount();
        colorIndex = nextColorIndex;

        SetUIColor();
    }

    public void SetCoordinateTextActive(bool isActive) {

        Vector2Int coordinates = attachedCoordinate.GetComponent<HexagonCoordinate>().coordinates;

        if (isActive) {
            _coordinateText.SetActive(true);
            //_coordinateText.GetComponent<TMPro.TextMeshProUGUI>().text = "(" + coordinates.x + ", " + coordinates.y + ")";
            _coordinateText.GetComponent<TMPro.TextMeshProUGUI>().text = "(" + coordinates.x + ", " + coordinates.y + ")";

        } else {
            _coordinateText.SetActive(false);
        }
    }
}
