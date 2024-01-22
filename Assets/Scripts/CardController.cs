using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController {
    private CardModel _model;
    private CardInteractor _interactor;
    private CardData _data;

    public CardController(CardInteractor creator, CardData data) {
        _model = new CardModel(data);
        _interactor = creator;
    }
    public void SetIndexInHand(int indexInHand) {
        _model.IndexInHand = indexInHand;
    }
    public void InitCard(int code = -1) {
        _model.Init(code);
    }
    public int GetIndexInHand() {
        return _model.IndexInHand;
    }
}
