using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour {
    [SerializeField] GameObject _enemyPrefab;
    Enemy[] _enemyPool;
    int _poolSize = 30;
    int _currentRound = 0;

    Enemy.MonsterType[,] _roundData = new Enemy.MonsterType[3, 10] { // TEMPORARY ONLY!!!
        {Enemy.MonsterType.RedSlime, Enemy.MonsterType.None, Enemy.MonsterType.None, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.None, Enemy.MonsterType.None, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.None, Enemy.MonsterType.None, Enemy.MonsterType.None},
        {Enemy.MonsterType.RedSlime, Enemy.MonsterType.None, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.None, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.None, Enemy.MonsterType.None, Enemy.MonsterType.RedSlime },
        {Enemy.MonsterType.RedSlime, Enemy.MonsterType.RedSlime, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.BlueSlime, Enemy.MonsterType.GreenSlime, Enemy.MonsterType.None, Enemy.MonsterType.RedSlime }
    };

    int[] _roundLength = new int[3] { 10, 10, 10 };

    void Awake() {
        InitializeEnemyPool();
    }

    void InitializeEnemyPool() {
        _enemyPool = new Enemy[_poolSize];
        for (int i = 0; i < _poolSize; ++i) {
            _enemyPool[i] = Instantiate(_enemyPrefab, transform).GetComponent<Enemy>();
            _enemyPool[i].gameObject.SetActive(false);
        }
    }

    public void PlayRound() {
        if (IsRoundPlaying())
            return;
        StartCoroutine(ProcessRound());
    }
    IEnumerator ProcessRound() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < _roundLength[_currentRound]; ++i) {
            Enemy.MonsterType monsterTypeToCreate = _roundData[_currentRound, i];
            EnableEnemyInPool(monsterTypeToCreate);
            yield return new WaitForSeconds(1);
        }

        while (IsRoundPlaying()) {
            yield return new WaitForSeconds(1);
        }

        _currentRound++;
        Debug.Log("Round Finished");
        yield return null;
    }

    void EnableEnemyInPool(Enemy.MonsterType monsterType) {
        if (monsterType == Enemy.MonsterType.None)
            return;
        for (int i = 0; i < _poolSize; ++i) {
            if (!_enemyPool[i].gameObject.activeInHierarchy) {
                _enemyPool[i].gameObject.SetActive(true);
                _enemyPool[i].SetMonsterType(monsterType);
                _enemyPool[i].InitializeEnemy();
                return;
            }
        }
    }
    bool IsRoundPlaying() {
        for (int i = 0; i < _poolSize; ++i) {
            if (_enemyPool[i].gameObject.activeInHierarchy) {
                return true;
            }
        }
        return false;
    }

    // Test Code
    public void SetCurrentRound(int round) {
        _currentRound = round;
        Debug.Log("Current Round is set to: " + _currentRound.ToString());
    }

}
