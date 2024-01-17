using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimController : MonoBehaviour {
    void OnEnable() {
    }

    void Update() {
        transform.position = new Vector3((Input.mousePosition.x / 1280f * 18f) - 9f, (Input.mousePosition.y / 720f * 10f) - 5f, 0f);
    }
}
