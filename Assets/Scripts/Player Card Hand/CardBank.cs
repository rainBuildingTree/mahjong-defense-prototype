using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBank : MonoBehaviour {
    protected int _size;
    protected Card[] _bank;
    protected int _numDeposit;
    protected PlayerDeckManager _manager;
    
    protected void Awake() {
        _manager = GetComponent<PlayerDeckManager>();
    }
    public virtual void Init(int size = 14) {
        _size = size;
        _numDeposit = 0;
        _bank = new Card[_size];
    }
    public void Deposit(Card card) {
        if (card == null)
            return;
        for (int i = 0; i < _size; ++i) {
            if (_bank[i] != null)
                continue;
            _bank[i] = card;
            _bank[i].gameObject.SetActive(false);
            _numDeposit++;
            return;
        }
        Debug.Log("Deposited");
    }
    public Card Withdraw() {
        for (int i = 0; i < _size; ++i) {
            if (_bank[i] == null)
                continue;
            Card cardToWithdraw = _bank[i];
            _bank[i] = null;
            cardToWithdraw.gameObject.SetActive(true);
            return cardToWithdraw;
        }
        return null;
    }
}
