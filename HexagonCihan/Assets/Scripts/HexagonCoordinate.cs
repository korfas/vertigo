using System.Collections;
using System.Collections.Generic;
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

    /*
    public void SlideBottom(int step) {

        int newX = coordinates.x + step;

        Vector3 newPos = HexagonCoordinates.Instance.GetCoordinatePosition(newX, coordinates.y);

        LeanTween.moveY(attachedHexagon, newPos.y, 0.2f).setOnComplete(()=> {

            if (coordinates.x == 0) {
                GameGrid.Instance.InstantiateNewHexagon(transform.position);
                Debug.Log("Instantiate 1");
            }
            if (coordinates.x == 1 && step == 2) {
                GameGrid.Instance.InstantiateNewHexagon(transform.position);
                Debug.Log("Instantiate 2");
            }

        });
    }
    */
}
