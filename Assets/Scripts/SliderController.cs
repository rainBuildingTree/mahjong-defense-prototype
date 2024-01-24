using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {
    float _timer = 3f;
    float _time = 3f;
    Slider _slider;
    void Awake() {
        _slider = GetComponent<Slider>();
        _slider.value = _time / _timer;
    }
    void Update() {
        if (_time < _timer) {
            _time += Time.deltaTime;
            if (_time > _timer)
                _time = _timer;
            _slider.value = _time / _timer;
        }
    }

    public void ResetTimer() {
        _time = 0f;
    }
}
