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
        [SerializeField] private SceneLoader _sceneLoader;

        private void Start()
        {
            _mainButton.onClick.AddListener(OnMainButtonClicked);

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
            _mainButton.onClick.RemoveListener(OnMainButtonClicked);

            if (IsNewGame())
            {
                _mainButton.onClick.RemoveListener(OnNewButtonClicked);
            }
            else
            {
                _mainButton.onClick.RemoveListener(OnContinueButtonClicked);
            }
        }

        private void OnMainButtonClicked()
        {
            _soundtrackAudioFade.StartFadeOut();
            _uiClickBlocker.SetActive(true);
        }

        private void OnNewButtonClicked()
        {
            _sceneLoader.LoadIntro();
        }

        private void OnContinueButtonClicked()
        {
            _sceneLoader.LoadLevel(PlayerPrefs.GetInt(SceneLoader.CurrentLevelSavingName, 0));
        }

        private bool IsNewGame()
        {
            return PlayerPrefs.GetInt(SceneLoader.CurrentLevelSavingName, 0) == 0;
        }
    }
}

