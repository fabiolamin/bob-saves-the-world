using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BSTW.Utils
{
    public class SceneLoader : MonoBehaviour
    {
        private const int MainMenuIndex = 0;
        private const int IntroIndex = 1;
        private const int Level1Index = 2;
        private const int Level2Index = 3;
        private const int Level3Index = 4;

        [SerializeField] private GameObject _loadingBg;
        [SerializeField] private float _loadingDelay = 2f;
        [SerializeField] private UnityEvent _onLoadingStart;

        private IEnumerator LoadScene(int index)
        {
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
            StartCoroutine(LoadScene(IntroIndex));
        }

        public void LoadLevel1()
        {
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

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

