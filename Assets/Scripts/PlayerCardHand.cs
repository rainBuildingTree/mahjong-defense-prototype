using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerCardHand : MonoBehaviour {
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Sprite cardSprites;

    const int HandSize = 14;
    const int NumTotalCardKind = 34;


    Card[] handCards;


    void Awake() {
        handCards = new Card[HandSize];
        InitializeHand();
    }
    public void InitializeHand() {
        for (int i = 0; i < HandSize; ++i) {
            handCards[i] = Instantiate(cardPrefab, transform).GetComponent<Card>();
            handCards[i].Initialize(Random.Range(0, NumTotalCardKind));
        }
        SortCards();
    }

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
            handCards[i].transform.position = new Vector3(instantiateWidthPosition, 0, 0);
            instantiateWidthPosition += handCards[i].GetComponent<RectTransform>().rect.width;
            if (i == HandSize - 2)
                instantiateWidthPosition = 1280 - handCards[i].GetComponent<RectTransform>().rect.width;
        }
    }
}
