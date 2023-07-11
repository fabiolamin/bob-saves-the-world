using BSTW.Audio;
using BSTW.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BSTW.UI
{
    public class MainButtonController : MonoBehaviour
    {
        [SerializeField] private Button _mainButton;
        [SerializeField] private TextMeshProUGUI _mainButtonText;
        [SerializeField] private AudioFade _soundtrackAudioFade;
        [SerializeField] private GameObject _uiClickBlocker;
        [SerializeField] private GameObject _howToPlayPanel;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private CursorController _cursorController;

        private void Start()
        {
            if (IsNewGame())
            {
                _mainButtonText.text = "new game";
                _mainButton.onClick.AddListener(OnNewButtonClicked);
            }
            else
            {
                _mainButtonText.text = "continue";
                _mainButton.onClick.AddListener(OnContinueButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (IsNewGame())
            {
                _mainButton.onClick.RemoveListener(OnNewButtonClicked);
            }
            else
            {
                _mainButton.onClick.RemoveListener(OnContinueButtonClicked);
            }
        }

        private void OnNewButtonClicked()
        {
            _howToPlayPanel.SetActive(true);
        }

        private void OnContinueButtonClicked()
        {
            _cursorController.gameObject.SetActive(false);
            _soundtrackAudioFade.StartFadeOut();
            _uiClickBlocker.SetActive(true);
            _sceneLoader.LoadLevel(PlayerPrefs.GetInt(SceneLoader.CurrentLevelSavingName, 0));
        }

        private bool IsNewGame()
        {
            return PlayerPrefs.GetInt(SceneLoader.CurrentLevelSavingName, 0) == 0;
        }
    }
}

