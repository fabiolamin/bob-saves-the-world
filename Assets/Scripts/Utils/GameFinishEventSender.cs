using BSTW.Game;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Utils
{
    public class GameFinishEventSender : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGameFinish;

        private void OnEnable()
        {
            GameManager.OnGameFinished += OnGameFinished;
        }

        private void OnDisable()
        {
            GameManager.OnGameFinished -= OnGameFinished;
        }

        private void OnGameFinished()
        {
            _onGameFinish?.Invoke();
        }
    }
}

