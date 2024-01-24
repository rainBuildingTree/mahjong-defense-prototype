using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardSpriteStorage : ScriptableObject {
    public Sprite[] pyroCardSprites;
    public Sprite[] anemoCardSprites;
    public Sprite[] hydroCardSprites;
    public Sprite[] charCardSprites;
    public Sprite[] pyroSequenceCardSprites;
    public Sprite[] anemoSequenceCardSprites;
    public Sprite[] hydroSequenceCardSprites;
    public Sprite[] pyroTripletCardSprites;
    public Sprite[] anemoTripletCardSprites;
    public Sprite[] hydroTripletCardSprites;
    public Sprite[] charTripletCardSprites;

    public Sprite GetSprite(Card.ElementalAttribute attribute, int number) {
        switch (attribute) {
            case Card.ElementalAttribute.Pyro:
                return pyroCardSprites[number-1];
            case Card.ElementalAttribute.Anemo:
                return anemoCardSprites[number-1];
            case Card.ElementalAttribute.Hydro:
                return hydroCardSprites[number-1];
            case Card.ElementalAttribute.Char:
                return charCardSprites[number-1];
            default:
                return null;
        }
    }
    public Sprite GetSprite(Card.ElementalAttribute attribute, MergedCard.MergeType type, int number) {
        switch (attribute) {
            case Card.ElementalAttribute.Pyro:
                return (type == MergedCard.MergeType.Sequence) ? pyroSequenceCardSprites[number-1] : pyroTripletCardSprites[number-1];
            case Card.ElementalAttribute.Anemo:
                return (type == MergedCard.MergeType.Sequence) ? anemoSequenceCardSprites[number-1] : anemoTripletCardSprites[number-1];
            case Card.ElementalAttribute.Hydro:
                return (type == MergedCard.MergeType.Sequence) ? hydroSequenceCardSprites[number-1] : hydroTripletCardSprites[number-1];
            case Card.ElementalAttribute.Char:
                return charTripletCardSprites[number-1];
            default:
                return null;
        }
    }
}
