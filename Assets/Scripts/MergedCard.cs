using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MergedCard : Card {
    public enum MergeType {Sequence, Triplet, Quad, None}

    private MergedCard _next = null;
    private MergedCard _prev = null;
    public MergedCard next { get { return _next;} }
    public MergedCard prev { get { return _prev;} }

    private const int NumMergedCardPerAttribute = 2;
    private const int NumTotalCardKind = 34;
    
    private MergeType _mergeType;
    

    public override void Initialize(int cardCode) {
        if (cardCode < 34 || cardCode > 41) {
            Debug.Log("Illegal merged card code of:\t" + cardCode.ToString());
            return;
        }

        _code = cardCode;
        // cardCode [0, NumTotalCardCount[: regualr cards' code
        // cardCode [NumTotalCardCount, 40]: merged cards' code
        _attribute = (ElementalAttribute)((cardCode - NumTotalCardKind) / NumMergedCardPerAttribute);
        _mergeType = (MergeType)((cardCode - NumTotalCardKind) % NumMergedCardPerAttribute);
    }
    public void Initialize(Card.ElementalAttribute attribute, MergeType type, int number) {
        _code = NumTotalCardKind + (int)attribute * 16 + (type == MergeType.Triplet ? 7 : 0) + number;
        _attribute = attribute;
        _mergeType = type;
        _number = number;
    }
    public void ConnectMergedCards(MergedCard prev, MergedCard next) {
        _prev = prev;
        _next = next;
    }
    public void SetMergedCardImage() {
        if (_prev != null) {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            imageComponent.sprite = null;
        }
        else {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _uiSize.x);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _uiSize.y);
            imageComponent.sprite = _spriteStorage.GetSprite(_attribute, _mergeType, _number);
        }
    }
    public MergedCard GetHead() {
        return (_prev == null) ? this : _prev.GetHead();
    }


}
