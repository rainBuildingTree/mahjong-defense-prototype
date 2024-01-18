using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerCardHand : MonoBehaviour {
    /* Loaded Objects */
    [SerializeField] GameObject cardPrefab;
    RectTransform rectTransform;
    AimController aimControl;

    /* Constants */
    const int HandSize = 14;
    const int NumTotalCardKind = 34;

    /* Data */
    Card[] handCards;

    /* Public Get/Set */
    public AimController AimControl { get { return aimControl; } }
    


    /* Unity Event Functions */
    void Awake() {
        handCards = new Card[HandSize];
        rectTransform = GetComponent<RectTransform>();
        aimControl = FindObjectOfType<AimController>();
        aimControl.gameObject.SetActive(false);
        InitializeHand();
    }



    /* Public Methods */
    public void InitializeHand() {
        for (int i = 0; i < HandSize; ++i) {
            handCards[i] = Instantiate(cardPrefab, transform).GetComponent<Card>();
            handCards[i].Initialize(Random.Range(0, NumTotalCardKind));
            handCards[i].RegisterPlayerCardHand(this);
        }
        SortCards();
    }
    public void SelectCard(int handIndex) {
        if (handIndex == HandSize - 1) {
            handCards[handIndex].Initialize(Random.Range(0, NumTotalCardKind));
            return;
        }
        Card switcher = handCards[HandSize - 1];
        handCards[HandSize - 1] = handCards[handIndex];
        handCards[handIndex] = switcher;

        handCards[HandSize - 1].Initialize(Random.Range(0, NumTotalCardKind));
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
            Debug.Log(instantiateWidthPosition);
            Debug.Log(handCards[i].rectTransform.position);
            instantiateWidthPosition += handCards[i].GetComponent<RectTransform>().rect.width;
            if (i == HandSize - 2)
                instantiateWidthPosition = rectTransform.rect.width - handCards[i].GetComponent<RectTransform>().rect.width;
        }
    }

}
