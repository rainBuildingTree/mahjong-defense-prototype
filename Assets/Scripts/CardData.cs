using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject {
    public Sprite[] pyroCardSprites;
    public Sprite[] anemoCardSprites;
    public Sprite[] hydroCardSprites;
    public Sprite[] charCardSprites;
    public Sprite[] mergedPyroCardSprites;
    public Sprite[] mergedAnemoCardSprites;
    public Sprite[] mergedHydroCardSprites;
    public Sprite[] mergedCharCardSprites;

    public int numCardPerAttribute = 9;
    public int numTotalCardCount = 34;
    public float uiMagnifyFactor = 1.1f;

    public enum ElementalAttribute { 
        Pyro, Anemo, Hydro, Char, 
        MergedPyro, MergedAnemo, MergedHydro, MergedChar 
    }
}
