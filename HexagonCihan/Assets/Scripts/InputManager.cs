using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager> {

    [SerializeField] private Button rotateCCW;
    [SerializeField] private Button rotateCW;

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
