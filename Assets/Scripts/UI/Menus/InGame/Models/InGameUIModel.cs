using StealthTD.GameLogic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Zenject;

namespace StealthTD.UI.InGame
{
	public class InGameUIModel : IInitializable, IDisposable, INotifyPropertyChanged
	{
		#region Private Fields

		[Inject]
		private readonly GameStateManager gameStateManager;

		[Inject]
		private readonly LevelManager levelManager;

		private bool hasReachedGoal;
		private bool isGameOver;
		private bool isPaused;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public int CurrentLevelIndex => levelManager.CurrentLevelIndex;

		public bool IsLastLevel => levelManager.IsLastLevel;

		public bool IsPaused
		{
			get => isPaused;
			set
			{
				isPaused = value;
				gameStateManager.SetIsGamePaused(value);
				InvokePropertyChanged();
			}
		}

		public bool IsGameOver
		{
			get => isGameOver;
			private set
			{
				isGameOver = value;
				InvokePropertyChanged();
			}
		}

		public bool HasReachedGoal
		{
			get => hasReachedGoal;
			private set
			{
				hasReachedGoal = value;
				InvokePropertyChanged();
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void LoadNextLevel() => levelManager.NextLevel();

		public void RestartCurrentLevel() => levelManager.RestartCurrentLevel();

		public void ExitToMainMenu() => levelManager.ExitToMainMenu();

		public string GetLevelCompleteText()
		{
			if (IsLastLevel && HasReachedGoal)
				return "Final level complete";
			else
				return $"Level {CurrentLevelIndex:00} complete";
		}

		public void Initialize()
		{
			gameStateManager.OnGameOver += GameStateManager_OnGameOver;
			gameStateManager.OnReachedGoal += GameStateManager_OnReachedGoal;
		}

		public void Dispose()
		{
			gameStateManager.OnGameOver -= GameStateManager_OnGameOver;
			gameStateManager.OnReachedGoal -= GameStateManager_OnReachedGoal;
		}

		#endregion Public Methods

		#region Private Methods

		private void InvokePropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		private void GameStateManager_OnReachedGoal()
		{
			HasReachedGoal = true;
		}

		private void GameStateManager_OnGameOver(bool isGameOver)
		{
			IsGameOver = isGameOver;
		}

		#endregion Private Methods
	}
}