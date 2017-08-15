﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private static InputManager _instance;
    public static ControlMode controlMode { get { return _instance.CurrentControlMode(); } }

    public GameObject[] controlModes;

    private ControlMode[] _controlModes;
    private int _controlModeIndex;
    
    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        _controlModes = controlModes
            .Select(InstatiateControlMode)
            .ToArray();
    }

    private ControlMode InstatiateControlMode(GameObject mode) {
        return Instantiate(mode, transform).GetComponent<ControlMode>();
    }

    protected ControlMode CurrentControlMode() {
        return _controlModes[_controlModeIndex];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) ||  Input.GetKeyDown(KeyCode.Joystick1Button7)) {
            GameManager.TogglePause();
            UIManager.TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            ToggleControlMode();
        }    
    }
    
    public static void ToggleControlMode() {
        _instance.IterateControlMode();
    }

    protected void IterateControlMode() {
        var oldIindex = _controlModeIndex;
        var newIndex = oldIindex == _controlModes.Length - 1 ? 
            0 : 
            oldIindex + 1;

        var oldMode = _controlModes[oldIindex];
        Destroy(oldMode);

        _controlModes[oldIindex] = InstatiateControlMode(controlModes[oldIindex]);
        _controlModeIndex = newIndex;
    }

    public static Vector3 GetMoveDirection() {
        return controlMode.GetMoveDirection();
    }

    public static Vector3 GetAimDirection() {
        return controlMode.GetAimDirection();
    }

    public static bool Attack() {
        return controlMode.Attack();
    }
}
