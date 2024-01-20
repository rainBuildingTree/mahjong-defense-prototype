using System;
using UnityEngine;


public class PlayerCardHand : MonoBehaviour {
    /* Loaded Objects */
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject mergedCardPrefab;
    RectTransform rectTransform;
    AimController aimControl;

    /* Constants */
    const int HandSize = 14;
    const int NumTotalCardKind = 34;

    /* Data */
    Card[] handCards;
    Card[] cardBank;
    MergedCard[] mergedCardBank;

    /* Public Get/Set */
    public AimController AimControl { get { return aimControl; } }
    


    /* Unity Event Functions */
    void Awake() {
        handCards = new Card[HandSize];
        cardBank = new Card[HandSize];
        mergedCardBank = new MergedCard[HandSize];
        rectTransform = GetComponent<RectTransform>();
        aimControl = FindObjectOfType<AimController>();
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
        if (handCards[handIndex] is MergedCard) {
            MergedCard prev = (MergedCard)handCards[handIndex];
            MergedCard head = null;
            while (prev != null) {
                head = prev;
                prev = head.prev;
            }
            MergedCard next = head.next;
            while(head != null) {
                int index = head.indexInHand;
                DepositCard(head);
                handCards[index] = WithdrawCard();
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
    public void MergeCards(int[] cardIndice, int cardCode) {
        Array.Sort(cardIndice);
        int numCards = 0;
        for (int i = 0; i < cardIndice.Length; ++i) {
            if (cardIndice[i] < 0 || cardIndice[i] >= NumTotalCardKind)
                continue;
            numCards++;
        }

        if (numCards < 3)
            return;

        for (int i = 0; i < numCards; ++i) {
            if (cardIndice[i] < 0) {
                i--;
                continue;
            }
            DepositCard(handCards[cardIndice[i]]);
            MergedCard withdrawnCard = WithdrawMergedCard();
            withdrawnCard.Initialize(cardCode);
            handCards[cardIndice[i]] = withdrawnCard;
        }
        
        for (int i = 0; i < numCards; ++i) {
            if (cardIndice[i] < 0) {
                i--;
                continue;
            }
            MergedCard prev = (i == 0) ? null : (MergedCard)handCards[cardIndice[i-1]];
            MergedCard next = (i == numCards - 1) ? null : (MergedCard)handCards[cardIndice[i+1]];
            ((MergedCard)handCards[cardIndice[i]]).ConnectMergedCards(prev, next);
            ((MergedCard)handCards[cardIndice[i]]).SetMergedCardImage();
        }

        SortCards();

    }



    /* Private Methods */
    void SortCards() {
        for (int i = 0; i < HandSize - 2; ++i) {
            for (int j = i+1; j < HandSize - 1; ++j) {
                if (handCards[i].code > handCards[j].code) {
                    Card switcher = handCards[i];
                    handCards[i] = handCards[j];
                    handCards[j] = switcher;
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
        Debug.Log("Sort Completed");
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
                Debug.Log("card withdrawn");
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
            Debug.Log("card deposited");
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
            DepositCard(mergedCardBank[i].next);
            mergedCardBank[i].gameObject.SetActive(false);
            return;
        }
    }
}
