using UnityEngine;

public class HexagonCoordinate : MonoBehaviour {

    public Vector2Int coordinates;
    public GameObject attachedHexagon = null;

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Hexagon") {

            attachedHexagon = other.gameObject;
            attachedHexagon.GetComponent<Hexagon>().attachedCoordinate = gameObject;

        }
    }

    public bool IsEmpty() {
        return attachedHexagon == null;
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.tag == "Hexagon") {

            attachedHexagon = null;

        }
    }

}
