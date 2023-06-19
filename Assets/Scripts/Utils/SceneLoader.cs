using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BSTW.Utils
{
    public class SceneLoader : MonoBehaviour
    {
        private bool _isLoading;

        [SerializeField] private GameObject _loadingBg;
        [SerializeField] private float _loadingDelay = 2f;
        [SerializeField] private UnityEvent _onLoadingStart;

        public static readonly string CurrentLevelSavingName = "CurrentLevel";

        public static readonly int MainMenuIndex = 0;
        public static readonly int IntroIndex = 1;
        public static readonly int Level1Index = 2;
        public static readonly int Level2Index = 3;
        public static readonly int Level3Index = 4;

        private IEnumerator LoadScene(int index)
        {
            if (_isLoading) yield break;

            _isLoading = true;

            Time.timeScale = 1f;

            _onLoadingStart?.Invoke();

            yield return new WaitForSeconds(_loadingDelay);

            _loadingBg.SetActive(true);

            var asyncLoadLevel = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            yield return new WaitUntil(() => asyncLoadLevel.isDone);

            _loadingBg.SetActive(false);
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadScene(MainMenuIndex));
        }

        public void LoadIntro()
        {
            PlayerPrefs.SetInt(CurrentLevelSavingName, IntroIndex);
            StartCoroutine(LoadScene(IntroIndex));
        }

        public void LoadLevel1()
        {
            PlayerPrefs.SetInt(CurrentLevelSavingName, Level1Index);
            StartCoroutine(LoadScene(Level1Index));
        }

        public void LoadLevel2()
        {
            StartCoroutine(LoadScene(Level2Index));
        }

        public void LoadLevel3()
        {
            StartCoroutine(LoadScene(Level3Index));
        }

        public void LoadLevel(int index)
        {
            StartCoroutine(LoadScene(index));
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

