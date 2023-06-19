using BSTW.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Game
{
    public class EnemyBaseDestructionLevelGoal : LevelGoal
    {
        private int _maxEnemyBases;

        [SerializeField] private List<GameObject> _enemyBases;

        [SerializeField] private UnityEvent _onEnemyBaseDestroyed;

        protected override void Awake()
        {
            _maxEnemyBases = _enemyBases.Count;

            base.Awake();
        }

        protected override bool IsGoalAchieved()
        {
            return _enemyBases.Count == 0;
        }

        public void RemoveEnemyBase(GameObject enemyBase)
        {
            if (_enemyBases.Contains(enemyBase))
            {
                _enemyBases.Remove(enemyBase);

                UpdateGoalText();

                _onEnemyBaseDestroyed?.Invoke();

                CheckGoalAchievement();
            }
        }

        protected override string GetGoalText()
        {
            return string.Format("{0}/{1}", _enemyBases.Count, _maxEnemyBases);
        }

        protected override void SaveAchievement()
        {
            PlayerPrefs.SetInt(SceneLoader.CurrentLevelSavingName, SceneLoader.Level2Index);
        }
    }
}

