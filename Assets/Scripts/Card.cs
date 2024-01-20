using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : 
MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
IBeginDragHandler, IDragHandler, IEndDragHandler {
    /* Enums *///==================================================
    protected enum ElementalAttribute { Pyro, Anemo, Hydro, Char, None }

    /* Member Variables *///==================================================
    // Loaded Componenets
    [SerializeField] private Sprite[] sprites;
    
    protected Image imageComponent;
    protected RectTransform _rectTransform;
    protected PlayerCardHand playerCardHand;

    // Constants
    protected const int NumCardPerAttribute = 9;

    // Variables
    protected ElementalAttribute _elementalAttribute;
    private int _number;
    protected int _code;
    protected int _indexInHand;
    protected Vector2 _uiSize;
    protected float magnificationFactor = 1.1f;

    // Public Get/Setter
    public RectTransform rectTransform { get { return _rectTransform; } }
    public int code { get { return _code; } }
    public int indexInHand { get {return _indexInHand; } }
    
    
    
    
    /* Unity Event Functions *///==================================================
    protected void Awake() {
        imageComponent = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _uiSize = _rectTransform.rect.size;
    }

    // Pointer Interface
    public void OnPointerEnter(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _uiSize.x * magnificationFactor);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _uiSize.y * magnificationFactor);
    }
    public void OnPointerExit(PointerEventData eventData) {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _uiSize.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _uiSize.y);   
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
    public virtual void Initialize(int cardCode) {
        if (cardCode > 33 || cardCode < 0) {
            Debug.Log("Illegal card code of:\t" + cardCode.ToString());
            return;
        }
        _code = cardCode;
        _elementalAttribute = (ElementalAttribute)(cardCode / NumCardPerAttribute);
        _number = (cardCode % NumCardPerAttribute) + 1;
        SetCardImage();
    }
    public void RegisterPlayerCardHand(PlayerCardHand hand) {
        playerCardHand = hand;
    }
    public void SetIndexInHand(int index) {
        _indexInHand = index;
    }


    /* Protected Methods *///==================================================
    protected virtual void SetCardImage() {
        int cardIndex = (int)_elementalAttribute * NumCardPerAttribute + (_number - 1);
        imageComponent.sprite = sprites[cardIndex];
    }

}
