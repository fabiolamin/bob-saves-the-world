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

        public static bool IsGamePaused { get; private set; }

        public static event Action<bool> OnGameResumed; 

        public void OnPause(InputAction.CallbackContext value)
        {
            if (value.started)
            {
                IsGamePaused = !IsGamePaused;

                if (IsGamePaused)
                {
                    _onGamePause?.Invoke();
                    OnGameResumed?.Invoke(false);
                    Time.timeScale = 0f;
                }
                else
                {
                    ResumeGame();
                }
            }
        }

        public void ResumeGame()
        {
            _onGameResume?.Invoke();
            OnGameResumed?.Invoke(true);
            Time.timeScale = 1f;
            IsGamePaused = false;
        }
    }
}

