using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BSTW.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        public static readonly string AudioVolumeName = "AudioVolume";

        private int _selectedQualityIndex;
        private int _selectedResolutionIndex;
        private Resolution _selectedResolution;
        private List<Resolution> _resolutions = new List<Resolution>();

        [SerializeField] private Slider _audioVolumeSlider;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        private void Awake()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(AudioVolumeName, 1f);

            GetResolutions();
            GetSettings();
        }

        public void GetSettings()
        {
            _selectedQualityIndex = QualitySettings.GetQualityLevel();

            var currentResolution = Screen.currentResolution;
            _selectedResolutionIndex = _resolutions.FindIndex(r => r.width == currentResolution.width &&
            r.height == currentResolution.height);

            _audioVolumeSlider.value = AudioListener.volume;
            _qualityDropdown.value = _selectedQualityIndex;
            _resolutionDropdown.value = _selectedResolutionIndex;
        }

        private void GetResolutions()
        {
            _resolutions.Clear();

            foreach (var resolution in Screen.resolutions)
            {
                if (_resolutions.Any(r => r.width == resolution.width && r.height == resolution.height)) continue;

                _resolutions.Add(resolution);
            }

            var resolutionsString = new List<string>();
            _resolutions.ForEach(r => resolutionsString.Add($"{r.width}X{r.height}"));

            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(resolutionsString);
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }

        public void SetQuality(int qualityIndex)
        {
            _selectedQualityIndex = qualityIndex;
        }

        public void SetResolution(int resolutionIndex)
        {
            if (_resolutions.Count == 0) return;

            _selectedResolutionIndex = resolutionIndex;
            _selectedResolution = _resolutions[_selectedResolutionIndex];
        }

        public void SaveSettings()
        {
            QualitySettings.SetQualityLevel(_selectedQualityIndex);
            Screen.SetResolution(_selectedResolution.width, _selectedResolution.height, Screen.fullScreen);

            PlayerPrefs.SetFloat(AudioVolumeName, AudioListener.volume);
        }
    }
}

