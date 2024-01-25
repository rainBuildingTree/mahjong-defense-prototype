using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergedCardBank : CardBank {
    public override void Init(int size = 14)
    {
        base.Init(size);
        Fill();
    }
    protected void Fill() {
        for (int i = 0; i < _size; ++i) {
            _bank[i] = Instantiate(_manager.MergedCardPrefab, transform).GetComponent<MergedCard>();
            _bank[i].RegisterDeck(_manager);
            _bank[i].gameObject.SetActive(false);
        }
        _numDeposit = _size;
    }
}
