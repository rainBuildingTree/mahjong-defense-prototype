using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInteractor : 
MonoBehaviour,
IPointerEnterHandler,
IPointerExitHandler,
IBeginDragHandler,
IDragHandler,
IEndDragHandler {
    [SerializeField] protected CardData _data;
    protected Image _image;
    protected RectTransform _rectTransform;
    protected PlayerCardHand _hand;
    protected Vector2 _uiSize;
    protected CardController _controller; 

    public void OnPointerEnter(PointerEventData eventData) {
        MagnifyCard(true);
    }
    public void OnPointerExit(PointerEventData eventData) {
        MagnifyCard(false);
    }
    public void OnBeginDrag(PointerEventData eventData) {
        EnableCardDrag(true);
    }
    public void OnDrag(PointerEventData eventData) {

    }
    public void OnEndDrag(PointerEventData eventData) {
        EnableCardDrag(false);
    }

    public void SetIndexInHand(int indexInHand) {
        _controller.SetIndexInHand(indexInHand);
    }

    public void Init(PlayerCardHand hand) {
        if (_controller != null) {
            _controller.InitCard();
            return;
        }
        _controller = new CardController(this, _data);
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _uiSize = _rectTransform.rect.size;
        _hand = hand;
        _controller.InitCard();
    }

    protected void MagnifyCard(bool isMagnify) {
        Vector2 newSize = isMagnify ? (_uiSize * _data.uiMagnifyFactor) : _uiSize;  
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.y);
    }
    protected void EnableCardDrag(bool isDragging) {
        _hand.AimControl.gameObject.SetActive(isDragging);
        _hand.SelectCard(isDragging ? -1 : _controller.GetIndexInHand());
    }
    protected void SetCardImage(CardData.ElementalAttribute attribute, int number) {
        switch (attribute) {
            case CardData.ElementalAttribute.Pyro:
                _image.sprite = _data.pyroCardSprites[number];
                break;
            case CardData.ElementalAttribute.Anemo:
                _image.sprite = _data.anemoCardSprites[number];
                break;
            case CardData.ElementalAttribute.Hydro:
                _image.sprite = _data.hydroCardSprites[number];
                break;
            case CardData.ElementalAttribute.MergedPyro:
                _image.sprite = _data.mergedPyroCardSprites[number];
                break;
            case CardData.ElementalAttribute.MergedAnemo:
                _image.sprite = _data.mergedAnemoCardSprites[number];
                break;
            case CardData.ElementalAttribute.MergedHydro:
                _image.sprite = _data.mergedHydroCardSprites[number];
                break;
            case CardData.ElementalAttribute.MergedChar:
                _image.sprite = _data.mergedCharCardSprites[number];
                break;
        }
    }
    

    
}
