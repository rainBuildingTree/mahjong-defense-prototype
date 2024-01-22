using System;
using UnityEngine;


public class PlayerCardHand : MonoBehaviour {
    /* Loaded Objects */
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject mergedCardPrefab;
    RectTransform rectTransform;
    AimController aimControl;
    CardMergeButton _mergeButton;

    /* Constants */
    const int HandSize = 14;
    const int NumTotalCardKind = 34;
    const int NumCardRequiredToMerge = 3;
    const int NumCardPerAttribute = 9;

    /* Data */
    Card[] handCards;
    Card[] cardBank;
    int[] cardMergeCandidateIndice;
    MergedCard[] mergedCardBank;
    bool _isMergeMode = false;
    int _numCardRegisteredToMerge = 0;

    /* Public Get/Set */
    public AimController AimControl { get { return aimControl; } }
    public bool IsMergeMode { get {return _isMergeMode;} }
    


    /* Unity Event Functions */
    void Awake() {
        handCards = new Card[HandSize];
        cardBank = new Card[HandSize];
        cardMergeCandidateIndice = new int[NumCardRequiredToMerge];
        mergedCardBank = new MergedCard[HandSize];
        rectTransform = GetComponent<RectTransform>();
        aimControl = FindObjectOfType<AimController>();
        _mergeButton = FindObjectOfType<CardMergeButton>();
        aimControl.gameObject.SetActive(false);
        InitializeHand();
    }



    /* Public Methods */
    public void InitializeHand() {
        for (int i = 0; i < HandSize; ++i) {
            handCards[i] = Instantiate(cardPrefab, transform).GetComponent<Card>();
            handCards[i].Initialize(UnityEngine.Random.Range(0, NumTotalCardKind));
            handCards[i].RegisterPlayerCardHand(this);

            mergedCardBank[i] = Instantiate(mergedCardPrefab, transform).GetComponent<MergedCard>();
            mergedCardBank[i].RegisterPlayerCardHand(this);
            mergedCardBank[i].gameObject.SetActive(false);
        }
        SortCards();
    }
    public void SelectCard(int handIndex) {
        Debug.Log("Select Card Start");
        if (handIndex < 0 || handIndex >= HandSize)
            return;
        Debug.Log("handIndex Validity Check Finished");
        if (handCards[handIndex] is MergedCard) {
            Debug.Log("MergedCard checked");
            MergedCard prev = (MergedCard)handCards[handIndex];
            MergedCard head = null;
            while (prev != null) {
                head = prev;
                prev = head.prev;
            }
            while(head != null) {
                int index = head.IndexInHand;
                DepositCard(head);
                Debug.Log("Deposited a merged card");
                handCards[index] = WithdrawCard();
                Debug.Log("Withdrew a card");
                handCards[index].Initialize(UnityEngine.Random.Range(0, NumTotalCardKind));
                if (head.next == null) {
                    Card tmp_switcher = handCards[HandSize - 1];
                    handCards[HandSize - 1] = handCards[index];
                    handCards[index] = tmp_switcher;
                }
                head = head.next;
            }
            SortCards();
            return;
        }

        if (handIndex == HandSize - 1) {
            handCards[handIndex].Initialize(UnityEngine.Random.Range(0, NumTotalCardKind));
            return;
        }
        Card switcher = handCards[HandSize - 1];
        handCards[HandSize - 1] = handCards[handIndex];
        handCards[handIndex] = switcher;

        handCards[HandSize - 1].Initialize(UnityEngine.Random.Range(0, NumTotalCardKind));

        SortCards();
    }
    public void RegisterCardToMerge(int cardIndexInHand) {
        if (!_isMergeMode)
            return;

        for (int i = 0; i < NumCardRequiredToMerge; ++i) {
            if (cardMergeCandidateIndice[i] == cardIndexInHand) {
                cardMergeCandidateIndice[i] = -1;
                handCards[cardIndexInHand].SetScreenerActive(true);
                if (_numCardRegisteredToMerge == NumCardRequiredToMerge)
                    _mergeButton.SetSprite(1);
                _numCardRegisteredToMerge--;
                return;
            }
        }
        if (_numCardRegisteredToMerge >= NumCardRequiredToMerge)
            return;
        if (cardIndexInHand < 0 || cardIndexInHand >= HandSize)
            return;
        for (int i = 0; i < NumCardRequiredToMerge; ++i) {
            if (cardMergeCandidateIndice[i] > -1)
                continue;
            cardMergeCandidateIndice[i] = cardIndexInHand;
            handCards[cardIndexInHand].SetScreenerActive(false);
            _numCardRegisteredToMerge++;
            if (IsMergeable(out MergedCard.MergeType type, out Card.ElementalAttribute attribute))
                _mergeButton.SetSprite(2);
            break;
        }
    }
    public void MergeCards() {
        MergedCard.MergeType mergeType;
        Card.ElementalAttribute elementalAttribute;
        if (!IsMergeable(out mergeType, out elementalAttribute))
            return;

        for (int i = 0; i < NumCardRequiredToMerge - 1; ++i) {
            for (int j = i+1; j < NumCardRequiredToMerge; ++j) {
                if (handCards[cardMergeCandidateIndice[i]].Code > handCards[cardMergeCandidateIndice[j]].Code) {
                    (cardMergeCandidateIndice[j], cardMergeCandidateIndice[i]) = (cardMergeCandidateIndice[i], cardMergeCandidateIndice[j]);
                }
            }
        }

        int cardCode = NumTotalCardKind + (int)elementalAttribute * 2 + (int)mergeType;


        for (int i = 0; i < _numCardRegisteredToMerge; ++i) {
            DepositCard(handCards[cardMergeCandidateIndice[i]]);
            MergedCard withdrawnCard = WithdrawMergedCard();
            withdrawnCard.Initialize(cardCode);
            handCards[cardMergeCandidateIndice[i]] = withdrawnCard;
        }
        
        for (int i = 0; i < _numCardRegisteredToMerge; ++i) {
            MergedCard prev = (i == 0) ? null : (MergedCard)handCards[cardMergeCandidateIndice[i-1]];
            MergedCard next = (i == _numCardRegisteredToMerge - 1) ? null : (MergedCard)handCards[cardMergeCandidateIndice[i+1]];
            ((MergedCard)handCards[cardMergeCandidateIndice[i]]).ConnectMergedCards(prev, next);
            ((MergedCard)handCards[cardMergeCandidateIndice[i]]).SetMergedCardImage();
        }

        SortCards();

    }
    public void ToggleMergeMode() {
        if (!_isMergeMode) {
            for (int i = 0; i < HandSize; ++i)
                handCards[i].SetScreenerActive(true);
            for (int i = 0; i < NumCardRequiredToMerge; ++i) {
                cardMergeCandidateIndice[i] = - 1;
            }
            _numCardRegisteredToMerge = 0;
            _mergeButton.SetSprite(1);
        }
        else {
            MergeCards();
            for (int i = 0; i < HandSize; ++i)
                handCards[i].SetScreenerActive(false);
            for (int i = 0; i < NumCardRequiredToMerge; ++i) {
                cardMergeCandidateIndice[i] = - 1;
            }
            _numCardRegisteredToMerge = 0;
             _mergeButton.SetSprite(0);
        }
        _isMergeMode = !_isMergeMode;
    }
    



    /* Private Methods */
    void SortCards() {
        for (int i = 0; i < HandSize - 2; ++i) {
            for (int j = i+1; j < HandSize - 1; ++j) {
                if (handCards[i].Code > handCards[j].Code) {
                    (handCards[j], handCards[i]) = (handCards[i], handCards[j]);
                }
            }
        }

        float instantiateWidthPosition = 0f;
        for (int i = 0; i < HandSize; ++i) {
            handCards[i].SetIndexInHand(i);
            handCards[i].rectTransform.anchoredPosition = new Vector3(instantiateWidthPosition, 0f, 0f);
            instantiateWidthPosition += handCards[i].GetComponent<RectTransform>().rect.width;
            if (i == HandSize - 2)
                instantiateWidthPosition = rectTransform.rect.width - handCards[HandSize - 1].GetComponent<RectTransform>().rect.width;
        }
    }
    MergedCard WithdrawMergedCard() {
        for (int i = 0; i < HandSize; ++i) {
            if (mergedCardBank[i] == null)
                continue;
            MergedCard cardToWithdraw = mergedCardBank[i];
            mergedCardBank[i] = null;
            cardToWithdraw.gameObject.SetActive(true);
            return cardToWithdraw;
        }
        return null;
    }
    Card WithdrawCard() {
        for (int i = 0; i < HandSize; ++i) {
            if (cardBank[i] == null)
                continue;
            Card cardToWithdraw = cardBank[i];
            cardBank[i] = null;
            if (cardToWithdraw != null)
            cardToWithdraw.gameObject.SetActive(true);
            return cardToWithdraw;
        }
        return null;
    }
    void DepositCard(Card cardToDeposit) {
        if (cardToDeposit == null)
            return;
            
        for (int i = 0; i < HandSize; ++i) {
            if (cardBank[i] != null)
                continue;
            cardBank[i] = cardToDeposit;
            cardBank[i].gameObject.SetActive(false);
            return;
        }
    }
    void DepositCard(MergedCard cardToDeposit) {
        if (cardToDeposit == null)
            return;

        for (int i = 0; i < HandSize; ++i) {
            if (mergedCardBank[i] != null)
                continue;
            mergedCardBank[i] = cardToDeposit;
            mergedCardBank[i].gameObject.SetActive(false);
            return;
        }
    }
    bool IsMergeable(out MergedCard.MergeType mergeType, out Card.ElementalAttribute elementalAttribute) {
        mergeType = MergedCard.MergeType.None;
        elementalAttribute = Card.ElementalAttribute.None;
        if (!_isMergeMode || _numCardRegisteredToMerge < NumCardRequiredToMerge) {
            return false;
        }
        bool isMergeable = true;
        for (int i = 0; i < NumCardRequiredToMerge - 1; ++i) {
            for (int j = i+1; j < NumCardRequiredToMerge; ++j) {
                if (handCards[cardMergeCandidateIndice[i]].Code > handCards[cardMergeCandidateIndice[j]].Code) {
                    (cardMergeCandidateIndice[j], cardMergeCandidateIndice[i]) = (cardMergeCandidateIndice[i], cardMergeCandidateIndice[j]);
                }
            }
        }
        int checker = handCards[cardMergeCandidateIndice[0]].Code / NumCardPerAttribute;
        for (int i = 1; i < NumCardRequiredToMerge; ++i) {
            if (checker != handCards[cardMergeCandidateIndice[i]].Code / NumCardPerAttribute)
                return false;
        }
        switch (checker) {
            case 0:
                elementalAttribute = Card.ElementalAttribute.Pyro;
                break;
            case 1:
                elementalAttribute = Card.ElementalAttribute.Anemo;
                break;
            case 2:
                elementalAttribute = Card.ElementalAttribute.Hydro;
                break;
            case 3:
                elementalAttribute = Card.ElementalAttribute.Char;
                break;
            default:
                elementalAttribute = Card.ElementalAttribute.None;
                break;
        }

        checker = handCards[cardMergeCandidateIndice[0]].Code;
        for (int i = 1; i < NumCardRequiredToMerge; ++i) {
            if (checker != handCards[cardMergeCandidateIndice[i]].Code)
                isMergeable = false;
        }
        if (isMergeable) {
            mergeType = MergedCard.MergeType.Triplet;
            return true;
        }
        isMergeable = true;
        checker = handCards[cardMergeCandidateIndice[0]].Code;
        for (int i = 1; i < NumCardRequiredToMerge; ++i) {
            if (checker != handCards[cardMergeCandidateIndice[i]].Code - i)
                isMergeable = false;
        }
        if (isMergeable)
            mergeType = MergedCard.MergeType.Sequence;
        return isMergeable;
    }
}
