using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    /* ENUMS *///==================================================
    public enum MonsterType { RedSlime, GreenSlime, BlueSlime, None }
    public enum ElementalAttribute { Pyro, Anemo, Hydro, None }



    /* MEMBER VARIABLES *///==================================================
    [SerializeField] Sprite[] _sprites;

    SpriteRenderer _spriteRenderer;
    EnemyMovement _movement;

    MonsterType _monsterType = MonsterType.None;
    ElementalAttribute _elementalAttribute = ElementalAttribute.None;
    


    /* UNITY EVENT FUNCTIONS *///==================================================
    void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
    }



    /* PUBLIC METHODS *///==================================================
    public void SetMonsterType(MonsterType monsterType) {
        _monsterType = monsterType;
        LoadEnemyData();
        LoadEnemySprite();
    }
    public void InitializeEnemy() {
        _movement.InitializeMovement();
    }



    /* PRIVATE METHODS *///==================================================
    void LoadEnemyData() {
        switch (_monsterType) {
            case MonsterType.RedSlime:
                _elementalAttribute = ElementalAttribute.Pyro;
                break;
            case MonsterType.GreenSlime:
                _elementalAttribute = ElementalAttribute.Anemo;
                break;
            case MonsterType.BlueSlime:
                _elementalAttribute = ElementalAttribute.Hydro;
                break;
            default:
                _elementalAttribute = ElementalAttribute.None;
                break;
        }
    }
    void LoadEnemySprite() {
        _spriteRenderer.sprite = _sprites[(int)_monsterType];
    }

}
