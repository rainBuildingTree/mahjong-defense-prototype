using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour {
    /* MEMBER VARIABLES *///==================================================
    [SerializeField] Card _target0;



    /* UNITY EVENT FUNCTIONS *///==================================================
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
            _target0.Initialize(0);
        else if (Input.GetKeyDown(KeyCode.W))
            _target0.Initialize(13);
        else if (Input.GetKeyDown(KeyCode.E))
            _target0.Initialize(25);
        else if (Input.GetKeyDown(KeyCode.R))
            _target0.Initialize(31);
        else if (Input.GetKeyDown(KeyCode.T))
            _target0.Initialize(69);
    }
}
