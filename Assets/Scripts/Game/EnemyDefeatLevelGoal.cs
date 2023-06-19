using BSTW.Enemy;
using BSTW.Utils;
using UnityEngine;

namespace BSTW.Game
{
    public class EnemyDefeatLevelGoal : LevelGoal
    {
        [SerializeField] private EnemyHealth _enemy;

        protected override string GetGoalText()
        {
            return string.Empty;
        }

        protected override bool IsGoalAchieved()
        {
            return !_enemy.IsAlive;
        }

        public void OnEnemyHit()
        {
            CheckGoalAchievement(); 
        }

        protected override void SaveAchievement()
        {
            PlayerPrefs.SetInt(SceneLoader.CurrentLevelSavingName, SceneLoader.MainMenuIndex);
        }
    }
}

