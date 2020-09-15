using System;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager> {

    [SerializeField] private Button rotateCCW = null;
    [SerializeField] private Button rotateCW = null;

    public event Action<string> OnRotated;

    private void Start() {

        rotateCCW.onClick.AddListener(HandleRotateCCWClicked);
        rotateCW.onClick.AddListener(HandleRotateCWClicked);
    }

    private void HandleRotateCCWClicked() {

        OnRotated?.Invoke("ccw");
    }

    private void HandleRotateCWClicked() {

        OnRotated?.Invoke("cw");
    }
}
