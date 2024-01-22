using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardMergeButton : MonoBehaviour {
    protected Image _image;
    [SerializeField] protected Sprite[] _sprites;

    void Awake() {
        _image = GetComponent<Image>();
    }

    public void SetSprite(int index) {
        _image.sprite = _sprites[index];
    }
}
