using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        private const int LEVEL_START = 1;
        
        public int CurrentLevel { get; private set; }

        private int LevelCount => SceneManager.sceneCountInBuildSettings - 1;
        private int currentLevelIndex;

        private void Start()
        {
            NextLevel();
        }

        public void NextLevel()
        {
            IncrementCurrentIndex();
            StartCoroutine(LoadLevelCoroutine(Util.SceneNameFromIndex(currentLevelIndex)));
        }
        
        private IEnumerator LoadLevelCoroutine(string levelName)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
            while (async.progress <= 0.89f)
            {
                yield return null;
            }
        }

        private void IncrementCurrentIndex()
        {
            currentLevelIndex++;
            if (currentLevelIndex > LevelCount)
                currentLevelIndex = LEVEL_START;

            CurrentLevel++;
        }
        
        
    }
}
