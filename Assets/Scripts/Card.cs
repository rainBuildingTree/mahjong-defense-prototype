using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : 
MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
IBeginDragHandler, IDragHandler, IEndDragHandler {
    /* Enums *///==================================================
    enum ElementalAttribute { Pyro, Anemo, Hydro, Char, None }

    /* Member Variables *///==================================================
    // Loaded Componenets
    [SerializeField] Sprite[] sprites;
    
    Image imageComponent;
    RectTransform _rectTransform;
    PlayerCardHand playerCardHand;

    // Constants
    const int CardNumberPerAttribute = 9;

    // Variables
    ElementalAttribute _elementalAttribute;
    int _number;
    int _code;
    int _indexInHand;

    // Public Get/Setter
    public RectTransform rectTransform { get { return _rectTransform; } }
    public int code { get { return _code; } }
    
    
    
    
    /* Unity Event Functions *///==================================================
    void Awake() {
        imageComponent = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    // Pointer Interface
    public void OnPointerEnter(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 99);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 150);
    }
    public void OnPointerExit(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 135);   
    }

    // Drag and Drop Interface
    public void OnDrag(PointerEventData eventData) { }
    public void OnBeginDrag(PointerEventData eventData) {
        playerCardHand.AimControl.gameObject.SetActive(true);
    }
    public void OnEndDrag(PointerEventData eventData) {
        playerCardHand.AimControl.gameObject.SetActive(false);
        playerCardHand.SelectCard(_indexInHand);
    }



    /* Public Methods *///==================================================
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


    /* Private Methods *///==================================================
    void SetCardImage() {
        int cardIndex = (int)_elementalAttribute * CardNumberPerAttribute + (_number - 1);
        imageComponent.sprite = sprites[cardIndex];
    }

}
