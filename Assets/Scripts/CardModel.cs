using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel {
    protected const int NumCardPerAttribute = 9;
    protected const int NumTotalCardCount = 34;
    protected int _code;
    protected int _indexInHand;
    protected CardData _data;
    protected CardData.ElementalAttribute _elementalAttribute;
    protected int _number;
    protected CardController _controller;

    public int IndexInHand { get; set; }

    public CardModel(CardData data) {
        _data = data;
    }
    public void Init(int code) {
        if (code < 0) {
            _code = UnityEngine.Random.Range(0, NumTotalCardCount);
            _elementalAttribute = (CardData.ElementalAttribute)(_code / NumCardPerAttribute);
            _number = (_code % NumCardPerAttribute) + 1;
            return;
        }
        if (!IsCodeValid(code))
            return;
        
        _code = code;
        _elementalAttribute = (CardData.ElementalAttribute)(code / NumCardPerAttribute);
        _number = (code % NumCardPerAttribute) + 1;
    }
    public (CardData.ElementalAttribute, int) GetCardData() {
        return (_elementalAttribute, _number);
    }

    protected bool IsCodeValid(int code) {
        return (code < 0 || code >= NumTotalCardCount) ? false : true;
    }
}
