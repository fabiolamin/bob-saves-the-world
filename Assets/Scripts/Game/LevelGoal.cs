using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

namespace BSTW.Game
{
    public abstract class LevelGoal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _goalTMP;

        [SerializeField] private UnityEvent _onGoalAchieved;
        [SerializeField] private UnityEvent _onGoalFailed;

        [SerializeField] private float _transitionDelay = 1f;

        protected virtual void Awake()
        {
            UpdateGoalText();
        }

        protected void CheckGoalAchievement()
        {
            if (IsGoalAchieved())
            {
                StartCoroutine(StartGoalAchievement());
            }
        }

        protected void UpdateGoalText()
        {
            _goalTMP.text = GetGoalText();
        }

        protected abstract string GetGoalText();
        protected abstract bool IsGoalAchieved();
        protected abstract void SaveAchievement();

        public void SetGoalFailed()
        {
            StartCoroutine(StartGoalFailed());
        }

        private IEnumerator StartGoalFailed()
        {
            yield return new WaitForSeconds(_transitionDelay);

            _onGoalFailed?.Invoke();
        }

        private IEnumerator StartGoalAchievement()
        {
            yield return new WaitForSeconds(_transitionDelay);

            SaveAchievement();
            _onGoalAchieved?.Invoke();
        }
    }
}

