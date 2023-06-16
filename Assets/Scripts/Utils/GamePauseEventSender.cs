using BSTW.Game;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Utils
{
    public class GamePauseEventSender : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGamePause;
        [SerializeField] private UnityEvent _onGameResume;

        private void OnEnable()
        {
            GameManager.OnGameResumed += OnGameResumed;
        }

        private void OnDisable()
        {
            GameManager.OnGameResumed -= OnGameResumed;
        }

        private void OnGameResumed(bool isResumed)
        {
            if (!isResumed)
                _onGamePause.Invoke();
            else
                _onGameResume.Invoke();
        }
    }
}

