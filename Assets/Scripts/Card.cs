using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler{
    enum ElementalAttribute {Pyro, Anemo, Hydro, Char, None}
    [SerializeField] Sprite[] sprites;
    const int CardNumberPerAttribute = 9;

    ElementalAttribute _elementalAttribute;
    int _number;
    int _code;
    int _indexInHand;
    public int code { get { return _code; } }

    Image imageComponent;
    RectTransform _rectTransform;
    public RectTransform rectTransform { get { return _rectTransform; } }
    PlayerCardHand playerCardHand;

    void Awake() {
        imageComponent = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 99);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
    }
    public void OnPointerExit(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 135);   
    }
    public void OnPointerUp(PointerEventData eventData) {
        playerCardHand.SelectCard(_indexInHand);
    }
    public void OnPointerDown(PointerEventData eventData) { }

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
    public void RegisterPlayerCardHand(PlayerCardHand hand) {
        playerCardHand = hand;
    }
    public void SetIndexInHand(int index) {
        _indexInHand = index;
    }


    void SetCardImage() {
        int cardIndex = (int)_elementalAttribute * CardNumberPerAttribute + (_number - 1);
        imageComponent.sprite = sprites[cardIndex];
    }

}
