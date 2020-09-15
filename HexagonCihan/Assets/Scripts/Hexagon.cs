using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour {

    [SerializeField] private GameObject _selector = null;
    [SerializeField] private GameObject _coordinateText = null;

    public int colorIndex { get; private set; } = 0;

    public GameObject attachedCoordinate = null;

    private Colors _colors;

    public void Init() {

        _selector.SetActive(false);

        GameObject designManager = GameObject.FindGameObjectWithTag("DesignManager");

        _colors = designManager.GetComponent<Colors>();

        int randomColorIndex = Random.Range(0, _colors.GetCount());
        colorIndex = randomColorIndex;

        SetUIColor();
    }

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
            _coordinateText.GetComponent<TMPro.TextMeshProUGUI>().text = "(" + coordinates.x + ", " + coordinates.y + ")";

        } else {
            _coordinateText.SetActive(false);
        }
    }
}
