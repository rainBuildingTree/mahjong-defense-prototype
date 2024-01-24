using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class CardMerger : MonoBehaviour {
    protected const int TripletSize = 3;
    protected const int SequenceSize = 3;
    protected const int QuadSize = 4;
    protected int _size;
    protected int _numCandidates;
    protected int[] _candidateIndice;
    protected bool _isMergeMode = false;
    protected PlayerDeckManager _manager;
    protected Card.ElementalAttribute _expectedAttribute = Card.ElementalAttribute.None;
    protected MergedCard.MergeType _expectedMergeType = MergedCard.MergeType.None;
    protected int _expectedNumber = -1;

    public bool IsMergeMode { get { return _isMergeMode; } }

    protected void Awake() {
        _manager = GetComponent<PlayerDeckManager>();
    }
    public void Init(int size = 4) {
        _size = 4;
        _numCandidates = 0;
        _candidateIndice = new int[_size];
        for (int i = 0; i < _size; ++i)
            _candidateIndice[i] = -1;
    }
    public void ToggleMergeMode() {
        if (!_isMergeMode) {
            SetHandCardDimActive(true);
            _manager.MergeButton.SetSprite(1);
            ResetCandidates();
        }
        else {
            Merge();
            SetHandCardDimActive(false);
            _manager.MergeButton.SetSprite(0);
            ResetCandidates();
        }
        _isMergeMode = !_isMergeMode;
    }
    
    public void RegisterCandidate(int index) {
        if (_numCandidates >= _size) {
            return;
        }
        if (index < 0 || index > _manager.Data.NumTotalCardKind) {
            return;
        }
        for (int i = 0; i < _size; ++i) {
            if (_candidateIndice[i] == index) {
                SetHandCardDimActive(true, index);
                _candidateIndice[i] = -1;
                _numCandidates--;
                if (IsMergeable())
                    _manager.MergeButton.SetSprite(2);
                else
                    _manager.MergeButton.SetSprite(1); 
                return;
            }
        }
        for (int i = 0; i < _size; ++i) {
            if (_candidateIndice[i] < 0) {
                SetHandCardDimActive(false, index);
                _candidateIndice[i] = index;
                _numCandidates++;
                if (IsMergeable())
                    _manager.MergeButton.SetSprite(2);
                else
                    _manager.MergeButton.SetSprite(1);
                return;
            }
        }
    }
    public void ResetCandidates() {
        for (int i = 0; i < _size; ++i)
            _candidateIndice[i] = -1;
        _numCandidates = 0;
    }
    public bool IsMergeable() {
        if (!_isMergeMode)
            return false;
        SortCandidates();
        if (!IsAttributeAllSame()) {
            Debug.Log("Attribute issue");
            return false;
        }
        if (IsTriplet()) {
            _expectedMergeType = MergedCard.MergeType.Triplet;
            return true;
        }
        else if (IsSequence()) {
            _expectedMergeType = MergedCard.MergeType.Sequence;
            return true;
        }
        else if (IsQuad()) {
            _expectedMergeType = MergedCard.MergeType.Quad;
            return true;
        }
        ResetExpectations();
        Debug.Log("Number issue");
        return false;
    }
    public void Merge() {
        if (!IsMergeable())
            return;
        Debug.Log("Mergeable!");
        _expectedNumber = GetExpectedNumber();
        for (int i = 0; i < _size; ++i) {
            if (_candidateIndice[i] < 0)
                continue;
            _manager.Bank.Deposit(_manager.Hand.Cards[_candidateIndice[i]]);
            MergedCard withdrawn = (MergedCard)_manager.MergedBank.Withdraw();
            withdrawn.Initialize(_expectedAttribute, _expectedMergeType, _expectedNumber);
            _manager.Hand.Cards[_candidateIndice[i]] = withdrawn;
        }
        for (int i = 0; i < _size; ++i) {
            if (_candidateIndice[i] < 0)
                continue;
            MergedCard prev = (i == 0 || _candidateIndice[i-1] < 0) ? null : (MergedCard)_manager.Hand.Cards[_candidateIndice[i-1]];
            MergedCard next = (i == _size-1) ? null : (MergedCard)_manager.Hand.Cards[_candidateIndice[i+1]];
            ((MergedCard)_manager.Hand.Cards[_candidateIndice[i]]).ConnectMergedCards(prev, next);
            ((MergedCard)_manager.Hand.Cards[_candidateIndice[i]]).SetMergedCardImage();
        }
        _manager.Hand.Sort();
    }   
    protected void SetHandCardDimActive(bool isActive, int index = -1) {
        if (index > -1) {
            _manager.Hand.Cards[index].SetScreenerActive(isActive);
            return;
        }
        for (int i = 0; i < _manager.Hand.Size; ++i) {
            if (_manager.Hand.Cards[i] == null)
                continue;
            _manager.Hand.Cards[i].SetScreenerActive(isActive);
        }
    }
    protected void ResetExpectations() {
        _expectedAttribute = Card.ElementalAttribute.None;
        _expectedMergeType = MergedCard.MergeType.None;
        _expectedNumber = -1;
    }
    protected bool IsAttributeAllSame() {
        _expectedAttribute = (_candidateIndice[_size-1] < 0) ? Card.ElementalAttribute.None : _manager.Hand.Cards[_candidateIndice[_size-1]].Attribute;
        if (_expectedAttribute == Card.ElementalAttribute.None)
            return false;
        for (int i = _size-2; i >= 0; --i) {
            if (_candidateIndice[i] < 0)
                continue;
            if (_manager.Hand.Cards[_candidateIndice[i]].Attribute != _expectedAttribute) {
                _expectedAttribute = Card.ElementalAttribute.None;
                return false;
            }
        }
        return true;
    }
    protected bool IsTriplet() {
        if (_numCandidates != TripletSize)
            return false;
        int checker = _manager.Hand.Cards[_candidateIndice[_size-1]].Code;
        for (int i = _size-2; i >= 0; --i) {
            if (_candidateIndice[i] < 0)
                continue;
            if (_manager.Hand.Cards[_candidateIndice[i]].Code != checker)
                return false;
        }
        return true;
    }
    protected bool IsSequence() {
        if (_numCandidates != SequenceSize)
            return false;
        int checker = _manager.Hand.Cards[_candidateIndice[_size-1]].Code;
        for (int i = _size-2; i >= 0; --i) {
            if (_candidateIndice[i] < 0)
                continue;
            if (checker != _manager.Hand.Cards[_candidateIndice[i]].Code+1)
                return false;
            checker--;
        }
        return true;
    }
    protected bool IsQuad() {
        if (_numCandidates != QuadSize)
            return false;
        int checker = _manager.Hand.Cards[_candidateIndice[_size-1]].Code;
        for (int i = _size-2; i >= 0; --i) {
            if (i < 0)
                continue;
            if (_manager.Hand.Cards[_candidateIndice[i]].Code != checker)
                return false;
        }
        return true;
    }
    protected void SortCandidates() {
        for (int i = 1; i < _size; ++i) {
            int key = _candidateIndice[i];
            int keyCode = (_candidateIndice[i] < 0) ? -1 : _manager.Hand.Cards[_candidateIndice[i]].Code;
            int j = i - 1;
            int targetCode = (_candidateIndice[j] < 0) ? -1 : _manager.Hand.Cards[_candidateIndice[j]].Code;
            while ((j >= 0) && (keyCode < targetCode)) {
                _candidateIndice[j+1] = _candidateIndice[j];
                j--;
                if (j < 0)
                    break;
                targetCode = (_candidateIndice[j] < 0) ? -1 : _manager.Hand.Cards[_candidateIndice[j]].Code;
            }
            _candidateIndice[j+1] = key;
        }
    }
    protected int GetExpectedNumber() {
        for (int i = 0; i < _size; ++i) {
            if (_candidateIndice[i] < 0)
                continue;
            return _manager.Hand.Cards[_candidateIndice[i]].Number;
        }
        return -1;
    }
}
