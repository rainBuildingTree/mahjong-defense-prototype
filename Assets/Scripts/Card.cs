using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : 
MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
IBeginDragHandler, IDragHandler, IEndDragHandler,
IPointerDownHandler, IPointerUpHandler {
    /* Enums *///==================================================
    public enum ElementalAttribute { Pyro, Anemo, Hydro, Char, None }

    /* Member Variables *///==================================================
    // Loaded Componenets
    [SerializeField] protected CardSpriteStorage _spriteStorage;
    protected Image imageComponent;
    protected RectTransform _rectTransform;
    protected PlayerDeckManager _deck;
    

    // Constants
    protected const int NumCardPerAttribute = 9;

    // Variables
    protected ElementalAttribute _attribute;
    protected int _number;
    protected int _code;
    protected int _indexInHand;
    protected Vector2 _uiSize;
    protected float magnificationFactor = 1.1f;

    // Public Get/Setter
    public RectTransform rectTransform { get { return _rectTransform; } }
    public int Code { get { return _code; } }
    public int IndexInHand { get { return _indexInHand; } }
    public ElementalAttribute Attribute { get { return _attribute; } }
    public int Number { get { return _number; } }
    public Vector2 UISize { get { return _uiSize; } }
    
    
    
    
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
    public void OnPointerDown(PointerEventData eventData) {

    }
    public void OnPointerUp(PointerEventData eventData) {
        if (!_deck.Merger.IsMergeMode) {
            return;
        }
        _deck.Merger.RegisterCandidate(_indexInHand);
    }

    // Drag and Drop Interface
    public void OnDrag(PointerEventData eventData) { }
    public void OnBeginDrag(PointerEventData eventData) {
        if (!_deck.Merger.IsMergeMode)
            _deck.Aim.gameObject.SetActive(true);
    }
    public void OnEndDrag(PointerEventData eventData) {
        if (!_deck.Merger.IsMergeMode) {
            _deck.Aim.gameObject.SetActive(false);
            _deck.Hand.UseCard(_indexInHand);
        }
    }



    /* Public Methods *///==================================================
    public virtual void Initialize(int cardCode) {
        if (cardCode > 33 || cardCode < 0) {
            Debug.Log("Illegal card code of:\t" + cardCode.ToString());
            return;
        }
        _code = cardCode;
        _attribute = (ElementalAttribute)(cardCode / NumCardPerAttribute);
        _number = (cardCode % NumCardPerAttribute) + 1;
        SetCardImage();
    }
    public void RegisterDeck(PlayerDeckManager deck) {
        _deck = deck;
    }
    public void SetIndexInHand(int index) {
        _indexInHand = index;
    }
    public void SetScreenerActive(bool isActive) {
        if (isActive) {
            imageComponent.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else {
            imageComponent.color = Color.white;
        }
    }


    /* Protected Methods *///==================================================
    protected virtual void SetCardImage() {
        imageComponent.sprite = _spriteStorage.GetSprite(_attribute, _number);
    }

}
