using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthTD.GameLogic
{
	public class LevelManager : MonoBehaviour
	{
		#region Public Properties

		public int CompletedLevelsCount => PlayerPrefs.GetInt("CompletedLevelsCount", 0);

		public int CurrentLevelIndex => SceneManager.GetActiveScene().buildIndex;

		public bool IsLastLevel => CurrentLevelIndex == SceneManager.sceneCountInBuildSettings - 1;

		#endregion Public Properties

		#region Public Methods

		public void RestartCurrentLevel()
		{
			StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().buildIndex));
		}

		public void NextLevel()
		{
			int nextSceneIndex = CurrentLevelIndex + 1;
			LoadLevel(nextSceneIndex);
		}

		public void LoadLevel(int levelIndex)
		{
			StartCoroutine(LoadSceneRoutine(levelIndex));
		}

		public void ExitToMainMenu()
		{
			StartCoroutine(LoadSceneRoutine(0));
		}

		public void OnLevelComplete()
		{
			if (CurrentLevelIndex > CompletedLevelsCount)
				PlayerPrefs.SetInt("CompletedLevelsCount", CurrentLevelIndex);
		}

		#endregion Public Methods

		#region Private Methods

		private IEnumerator LoadSceneRoutine(int sceneIndex)
		{
			yield return new WaitForEndOfFrame();
			SceneManager.LoadScene(sceneIndex);
		}

		#endregion Private Methods
	}
}