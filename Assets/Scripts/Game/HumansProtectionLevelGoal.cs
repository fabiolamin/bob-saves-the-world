using BSTW.Utils;
using System;
using UnityEngine;
using System.Linq;

namespace BSTW.Game
{
    public class HumansProtectionLevelGoal : LevelGoal
    {
        [SerializeField] private Health[] _humans;
        [SerializeField] private float _durationInSeconds;

        private void Update()
        {
            UpdateGoalText();
            CheckTimer();
        }

        protected override string GetGoalText()
        {
            _durationInSeconds -= Mathf.Max(Time.deltaTime, 0f);

            TimeSpan time = TimeSpan.FromSeconds(_durationInSeconds);

            return time.ToString(@"mm\:ss");
        }

        protected override bool IsGoalAchieved()
        {
            return _humans.All(h => h.IsAlive);
        }

        private void CheckTimer()
        {
            if(_durationInSeconds <= 0f)
            {
                CheckGoalAchievement();
            }
        }

        protected override void SaveAchievement()
        {
            PlayerPrefs.SetInt(SceneLoader.CurrentLevelSavingName, SceneLoader.Level3Index);
        }
    }

}
