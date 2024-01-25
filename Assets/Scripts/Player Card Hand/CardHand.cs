using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class CardHand : MonoBehaviour {
    
    public const int NumTotalCardKind = 34;
    protected PlayerDeckManager _manager;
    protected int _size;
    protected Card[] _cards;
    protected int _numCards;

    public int Size { get { return _size; } }
    public Card[] Cards { get { return _cards; } }

    protected void Awake() {
        _manager = GetComponent<PlayerDeckManager>();
    }
    public void Init(int size = 14) {
        _size = size;
        _numCards = 0;
        _cards = new Card[_size];
        CreateCards();
        Sort();
        StartCoroutine(CardRegen());
    }
    public void CreateCards() {
        for (int i = 0; i < _size; ++i) {
            _cards[i] = Instantiate(_manager.CardPrefab, transform).GetComponent<Card>();
            _cards[i].Initialize(Random.Range(0, _manager.Data.NumTotalCardKind));
            _cards[i].RegisterDeck(_manager);
        }
        _numCards = _size;
    }
    public void Sort() { // Doesn't sort the last card
        for (int i = 1; i < _size-1; ++i) {
            Card key = _cards[i];
            int keyCode = (key == null) ? int.MaxValue : key.Code;
            int j = i-1;
            int targetCode = (_cards[j] == null) ? int.MaxValue : _cards[j].Code;
            while ((j >= 0) && (keyCode < targetCode)) {
                _cards[j+1] = _cards[j];
                j--;
                if (j < 0)
                    break;
                targetCode = (_cards[j] == null) ? int.MaxValue : _cards[j].Code;
            }
            _cards[j+1] = key;
        }
        AssignIndexToCards();
        Place();
    }
    public void AssignIndexToCards() {
        for (int i = 0 ; i < _size; ++i) {
            if (_cards[i] == null)
                continue;
            _cards[i].SetIndexInHand(i);
        }
    }
    public void Place() {
        float xPlacePosition = 0f;
        for (int i = 0; i < _size-1; ++i) {
            if (_cards[i] == null)
                continue;
            _cards[i].rectTransform.anchoredPosition = new Vector3(xPlacePosition, 0f, 0f);
            xPlacePosition += _cards[i].UISize.x;
        }
        if (_cards[_size-1] == null)
            return;
        xPlacePosition = _manager.RectTransform.rect.width - _cards[_size-1].UISize.x;
        _cards[_size-1].rectTransform.anchoredPosition = new Vector3(xPlacePosition, 0f, 0f);
    }
    public void Draw() {
        if (_numCards >= _size)
            return;
        for (int i = _size-2; i >= 0; --i)
            if (_cards[i] == null) {
                _cards[i] = _cards[_size-1];
                break;
            }
        _cards[_size-1] = _manager.Bank.Withdraw();
        _cards[_size-1].Initialize(Random.Range(0, NumTotalCardKind));
        _numCards++;
        Sort();
        Place();

    }
    public void UseCard(int index) {
        if (index < 0 || index >= _size)
            return;

        if (_cards[index] is MergedCard head) {
            head = head.GetHead();
            for (MergedCard p = head; p != null; p = head) {
                head = head.next;
                _manager.MergedBank.Deposit(p);
                _cards[p.IndexInHand] = null;
                _numCards--;
            }
        }
        else {
            _manager.Bank.Deposit(_cards[index]);
            _cards[index] = null;
            _numCards--;
        }
        Sort();
    }

    protected IEnumerator CardRegen() {
        yield return new WaitForEndOfFrame();
        while (true) {
            if (_numCards >= _size) {
                yield return new WaitForEndOfFrame();
                continue;
            }
            _manager.CardGenSlider.ResetTimer();
            yield return new WaitForSeconds(3f);
            while (_manager.Merger.IsMergeMode) {
                yield return new WaitForSeconds(1f);
            }
            Draw();
        }
    }

}
