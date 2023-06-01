using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BSTW.Utils
{
    public class SceneLoader : MonoBehaviour
    {
        private const int MAIN_MENU = 0;
        private const int INTRO = 1;
        private const int LEVEL_1 = 2;
        private const int LEVEL_2 = 3;
        private const int LEVEL_3 = 4;
        private const int CREDITS = 5;

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
            StartCoroutine(LoadScene(MAIN_MENU));
        }

        public void LoadIntro()
        {
            StartCoroutine(LoadScene(INTRO));
        }

        public void LoadLevel1()
        {
            StartCoroutine(LoadScene(LEVEL_1));
        }

        public void LoadLevel2()
        {
            StartCoroutine(LoadScene(LEVEL_2));
        }

        public void LoadLevel3()
        {
            StartCoroutine(LoadScene(LEVEL_3));
        }

        public void LoadCredits()
        {
            StartCoroutine(LoadScene(CREDITS));
        }
    }
}

