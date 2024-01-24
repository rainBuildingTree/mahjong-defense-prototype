using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour {
    /* MEMBER VARIABLES *///==================================================
    [SerializeField] EnemyObjectPool _target0;
    [SerializeField] PlayerDeckManager _target1;
    int[] indice = {0, 1, 2};



    /* UNITY EVENT FUNCTIONS *///==================================================
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
