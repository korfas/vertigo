using System;
using UnityEngine;

public class Hexagons : Singleton<Hexagons> {


    //private GameObject[] _allHexagons;
    /*
    public event Action OnInitialized;
    public bool isInitialized { get; private set; } = false;

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
    */
    /*
    public void Init() {

        int hexagonsCount = transform.childCount;
        _allHexagons = new GameObject[hexagonsCount];

        for (int i = 0; i < hexagonsCount; i++) {
            _allHexagons[i] = transform.GetChild(i).gameObject;
            _allHexagons[i].GetComponent<Hexagon>().Init();
        }

        isInitialized = true;
        OnInitialized?.Invoke();
    }

    public GameObject Get(int x, int y) {

        return transform.Find(x + "_" + y).gameObject;
    }

    public void DeactivateAllSelectors() {

        foreach (GameObject hexagon in _allHexagons) {
            if (hexagon != null) {
                hexagon.GetComponent<Hexagon>().SetSelectorActivate(false);
            }
        }
    }
    */

    
}
