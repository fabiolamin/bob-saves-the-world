using BSTW.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using BSTW.Enemy.AI;
using UnityEngine.AI;

namespace BSTW.Enemy
{
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        public TerrestrialEnemyAIController Enemy;
        public ObjectPooling EnemyPooling;
        [Range(0, 10)]
        public int MaxEnemiesAlive;
    }

    public class EnemySpawner : MonoBehaviour
    {
        private int _maxEnemiesAlive;

        private Coroutine _enemySpawnCoroutine;

        private List<TerrestrialEnemyAIController> _enemiesAlive = new List<TerrestrialEnemyAIController>();

        [SerializeField] private EnemySpawnInfo[] _enemySpawnInfos;

        [Tooltip("In seconds.")]
        [SerializeField] private float _spawnInterval = 60f;

        [SerializeField] private UnityEvent _onEnemySpawned;

        private void Start()
        {
            _maxEnemiesAlive = _enemySpawnInfos.Sum(e => e.MaxEnemiesAlive);

            _enemySpawnCoroutine = StartCoroutine(StartSpawn());
        }

        private IEnumerator StartSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                PrepareSpawn();
            }
        }

        private void PrepareSpawn()
        {
            if (_enemiesAlive.Count < _maxEnemiesAlive)
            {
                System.Random random = new System.Random();
                var randomEnemySpawnInfos = _enemySpawnInfos.OrderBy(x => random.Next()).ToArray();

                foreach (var enemySpawnInfo in randomEnemySpawnInfos)
                {
                    if (_enemiesAlive.Where(e => e.Id == enemySpawnInfo.Enemy.Id).Count() < enemySpawnInfo.MaxEnemiesAlive)
                    {
                        Spawn(enemySpawnInfo);

                        break;
                    }
                }
            }
        }

        private void Spawn(EnemySpawnInfo newSpawn)
        {
            _onEnemySpawned?.Invoke();

            var newEnemy = newSpawn.EnemyPooling.GetObject().GetComponent<TerrestrialEnemyAIController>();

            newEnemy.EnemySpawner = this;

            AddEnemyAlive(newEnemy);

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(transform.position, out navMeshHit, Mathf.Infinity, 1);

            newEnemy.gameObject.SetActive(true);
            newEnemy.transform.position = navMeshHit.position;
            newEnemy.RestoreEnemy();
        }

        public void RemoveEnemyAlive(TerrestrialEnemyAIController enemyController)
        {
            if (_enemiesAlive.Contains(enemyController))
            {
                if (_enemiesAlive.Count == _maxEnemiesAlive)
                {
                    if (_enemySpawnCoroutine != null)
                    {
                        StopCoroutine(_enemySpawnCoroutine);
                    }

                    _enemySpawnCoroutine = StartCoroutine(StartSpawn());
                }

                _enemiesAlive.Remove(enemyController);
            }
        }

        public void AddEnemyAlive(TerrestrialEnemyAIController enemyController)
        {
            if (_enemiesAlive.Contains(enemyController)) return;

            _enemiesAlive.Add(enemyController);

            if (_enemiesAlive.Count == _maxEnemiesAlive)
            {
                if (_enemySpawnCoroutine != null)
                {
                    StopCoroutine(_enemySpawnCoroutine);
                }
            }
        }
    }
}

