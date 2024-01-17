using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    enum ElementalAttribute {Pyro, Anemo, Hydro, Char, None}
    [SerializeField] Sprite[] sprites;
    const int CardNumberPerAttribute = 9;

    ElementalAttribute _elementalAttribute;
    int _number;
    int _code;
    public int code { get { return _code; } }

    Image imageComponent;
    RectTransform rectTransform;

    void Awake() {
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 99);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
    }
    public void OnPointerExit(PointerEventData eventData) {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 135);   
    }

    public void Initialize(int cardCode) {
        if (cardCode > 33 || cardCode < 0) {
            Debug.Log("Illegal card code of:\t" + cardCode.ToString());
            return;
        }
        _code = cardCode;
        _elementalAttribute = (ElementalAttribute)(cardCode / CardNumberPerAttribute);
        _number = (cardCode % CardNumberPerAttribute) + 1;
        SetCardImage();
    }


    void SetCardImage() {
        int cardIndex = (int)_elementalAttribute * CardNumberPerAttribute + (_number - 1);
        imageComponent.sprite = sprites[cardIndex];
    }

}
