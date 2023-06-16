using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BSTW.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGamePause;
        [SerializeField] private UnityEvent _onGameResume;
        [SerializeField] private UnityEvent _onGameFinish;

        public bool IsGamePaused { get; private set; }
        public bool IsGameFinished { get; private set; }

        public static event Action<bool> OnGameResumed;
        public static event Action OnGameFinished;

        public void OnPause(InputAction.CallbackContext value)
        {
            if (IsGameFinished) return;

            if (value.started)
            {
                IsGamePaused = !IsGamePaused;

                if (IsGamePaused)
                    PauseGame();
                else
                    ResumeGame();
            }
        }

        public void PauseGame()
        {
            _onGamePause?.Invoke();
            OnGameResumed?.Invoke(false);
            Time.timeScale = 0f;
            IsGamePaused = true;
        }

        public void ResumeGame()
        {
            _onGameResume?.Invoke();
            OnGameResumed?.Invoke(true);
            Time.timeScale = 1f;
            IsGamePaused = false;
        }

        public void FinishGame()
        {
            IsGameFinished = true;
            OnGameFinished?.Invoke();
            _onGameFinish?.Invoke();
        }
    }
}

