using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start() {

        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;

        float posX = GetComponent<RectTransform>().position.x;
        float posY = GetComponent<RectTransform>().rect.size.y;

        Debug.Log("Size: (" + width + ", " + height + ")");
        Debug.Log("Pos: (" + posX + ", " + posY + ")");
    }
}
