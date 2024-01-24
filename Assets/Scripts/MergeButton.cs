using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MergeButton : MonoBehaviour {
    [SerializeField] protected Sprite[] _sprites;
    protected Image _image;
    protected PlayerDeckManager _deck;

    protected void Awake() {
        _image = GetComponent<Image>();
        _deck = FindObjectOfType<PlayerDeckManager>();
    }
    public void SetSprite(int index) {
        _image.sprite = _sprites[index];
    }
    public void Click() {
        _deck.Merger.ToggleMergeMode();
    }
}
