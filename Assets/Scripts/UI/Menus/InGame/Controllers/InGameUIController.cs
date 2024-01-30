using StealthTD.Audio;
using StealthTD.GameLogic;
using StealthTD.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace StealthTD.UI.InGame
{
	public class InGameUIController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly PauseMenuView pauseMenuView;

		[Inject]
		private readonly GameOverMenuView gameOverMenuView;

		[Inject]
		private readonly LevelCompleteView levelCompleteView;

		[Inject]
		private readonly ObjectivesView objectivesView;

		[Inject]
		private readonly CursorController cursor;

		[Inject]
		private readonly MenuAudioManager menuAudioManager;

		[Inject]
		private readonly InputProvider inputProvider;

		[Inject]
		private readonly InGameUIModel model;

		[Inject]
		private readonly ObjectiveManager objectiveManager;

		#endregion Private Fields

		#region Private Methods

		private void Awake()
		{
			pauseMenuView.UpdateView(model.IsPaused, false);
			gameOverMenuView.UpdateView(model.IsGameOver, false);
			levelCompleteView.UpdateView(model.GetLevelCompleteText(), model.HasReachedGoal, model.IsLastLevel, false);
			cursor.SetCrosshair();
		}

		private void OnEnable()
		{
			model.PropertyChanged += Model_PropertyChanged;

			inputProvider.PauseGame.performed += PauseGame_performed;

			pauseMenuView.ResumeButton.onClick.AddListener(ResumeGame);
			pauseMenuView.RestartLevelButton.onClick.AddListener(RestartLevel);
			pauseMenuView.ExitToMenuButton.onClick.AddListener(ExitToMenu);

			gameOverMenuView.RestartLevelButton.onClick.AddListener(RestartLevel);
			gameOverMenuView.ExitToMenuButton.onClick.AddListener(ExitToMenu);

			levelCompleteView.NextLevelButton.onClick.AddListener(NextLevel);
			levelCompleteView.RestartLevelButton.onClick.AddListener(RestartLevel);
			levelCompleteView.ExitToMenuButton.onClick.AddListener(ExitToMenu);

			objectiveManager.OnObjectivesUpdated += ObjectiveManager_OnObjectivesUpdated;
		}

		private void ObjectiveManager_OnObjectivesUpdated()
		{
			objectivesView.UpdateView(
				objectiveManager.GetAllKeycards.EnableObjective,
				objectiveManager.KillAllEnemies.EnableObjective,
				objectiveManager.NoDetection.EnableObjective,
				objectiveManager.AllObjectivesCompleted,
				objectiveManager.CurrentKeycardsCount,
				objectiveManager.TotalKeycardsCount,
				objectiveManager.CurrentEnemiesCount,
				objectiveManager.TotalEnemiesCount,
				objectiveManager.WasDetected,
				false);
		}

		private void OnDisable()
		{
			model.PropertyChanged -= Model_PropertyChanged;

			inputProvider.PauseGame.performed -= PauseGame_performed;

			pauseMenuView.ResumeButton.onClick.RemoveListener(ResumeGame);
			pauseMenuView.RestartLevelButton.onClick.RemoveListener(RestartLevel);
			pauseMenuView.ExitToMenuButton.onClick.RemoveListener(ExitToMenu);

			gameOverMenuView.RestartLevelButton.onClick.RemoveListener(RestartLevel);
			gameOverMenuView.ExitToMenuButton.onClick.RemoveListener(ExitToMenu);

			levelCompleteView.NextLevelButton.onClick.RemoveListener(NextLevel);
			levelCompleteView.RestartLevelButton.onClick.RemoveListener(RestartLevel);
			levelCompleteView.ExitToMenuButton.onClick.RemoveListener(ExitToMenu);

			objectiveManager.OnObjectivesUpdated -= ObjectiveManager_OnObjectivesUpdated;
		}

		private void NextLevel()
		{
			model.IsPaused = false;
			menuAudioManager.PlayButtonPressAudio();
			model.LoadNextLevel();
		}

		private void ResumeGame()
		{
			model.IsPaused = false;
			menuAudioManager.PlayButtonPressAudio();
		}

		private void RestartLevel()
		{
			model.IsPaused = false;
			menuAudioManager.PlayButtonPressAudio();
			model.RestartCurrentLevel();
		}

		private void ExitToMenu()
		{
			model.IsPaused = false;
			menuAudioManager.PlayButtonPressAudio();
			model.ExitToMainMenu();
		}

		private void UpdateCursor(bool showArrow)
		{
			if (showArrow)
				cursor.SetArrowPointer();
			else
				cursor.SetCrosshair();
		}

		private void PauseGame_performed(InputAction.CallbackContext obj)
		{
			if (model.IsGameOver || model.HasReachedGoal)
				return;

			model.IsPaused = !model.IsPaused;
		}

		private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(model.IsPaused):
				menuAudioManager.PlayButtonPressAudio();
				pauseMenuView.UpdateView(model.IsPaused);
				UpdateCursor(model.IsPaused);
				break;

				case nameof(model.IsGameOver):
				gameOverMenuView.UpdateView(model.IsGameOver);
				UpdateCursor(model.IsGameOver);
				menuAudioManager.PlayGameOverAudio();
				break;

				case nameof(model.HasReachedGoal):
				levelCompleteView.UpdateView(model.GetLevelCompleteText(), model.HasReachedGoal, model.IsLastLevel);
				cursor.SetArrowPointer();
				menuAudioManager.PlayLevelCompleteAudio();

				objectivesView.UpdateView(
				objectiveManager.GetAllKeycards.EnableObjective,
				objectiveManager.KillAllEnemies.EnableObjective,
				objectiveManager.NoDetection.EnableObjective,
				objectiveManager.AllObjectivesCompleted,
				objectiveManager.CurrentKeycardsCount,
				objectiveManager.TotalKeycardsCount,
				objectiveManager.CurrentEnemiesCount,
				objectiveManager.TotalEnemiesCount,
				objectiveManager.WasDetected,
				true);
				break;
			}
		}

		#endregion Private Methods
	}
}