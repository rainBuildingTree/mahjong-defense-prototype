using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour {
    [SerializeField] EnemyObjectPool _target0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q))
            _target0.SetCurrentRound(0);
        else if (Input.GetKeyDown(KeyCode.W))
            _target0.SetCurrentRound(1);
        else if (Input.GetKeyDown(KeyCode.E))
            _target0.SetCurrentRound(2);
        else if (Input.GetKeyDown(KeyCode.R))
            _target0.PlayRound();
    }
}
