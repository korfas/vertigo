using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour {

    [SerializeField] private Color[] colors = null;

    public int GetCount() {
        return colors.Length;
    }

    public Color GetColor(int index) {

        return colors[index];
    }
    /*
    public Color GetRandomColor() {

        int i = Random.Range(0, colors.Length);
        return colors[i];
    }
    */

}
