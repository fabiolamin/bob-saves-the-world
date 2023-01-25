using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace BSTW.Game
{
    public abstract class LevelGoal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _goalTMP;

        [SerializeField] private UnityEvent _onGoalAchieved;
        [SerializeField] private UnityEvent _onGoalFailed;

        [TextArea(4, 6)]
        public string GoalText;

        protected virtual void Awake()
        {
            UpdateGoalText();
        }

        protected void CheckGoalAchievement()
        {
            if (IsGoalAchieved())
            {
                _onGoalAchieved?.Invoke();
            }
        }

        protected void UpdateGoalText()
        {
            _goalTMP.text = GetGoalText();
        }

        protected abstract string GetGoalText();
        protected abstract bool IsGoalAchieved();

        public void SetGoalFailed()
        {
            _onGoalFailed?.Invoke();
        }
    }
}

