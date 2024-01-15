using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum MonsterType { None, RedSlime, GreenSlime, BlueSlime }
    MonsterType _monsterType = MonsterType.None;
    public enum ElementalAttribute { None, Pyro, Anemo, Hydro }
    ElementalAttribute _elementalAttribute = ElementalAttribute.None;

    SpriteRenderer _spriteRenderer;
    EnemyMovement _movement;

    [SerializeField] Sprite[] _sprites;

    void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
    }

    public void SetMonsterType(MonsterType monsterType) {
        _monsterType = monsterType;
        LoadEnemyData();
        LoadEnemySprite();
    }
    public void InitializeEnemy() {
        _movement.InitializeMovement();
    }

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
