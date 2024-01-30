using StealthTD.Player;
using System;
using UnityEngine;
using Zenject;

namespace StealthTD.GameLogic
{
	public class GameStateManager : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly LevelManager levelManager;

		[Inject]
		private readonly PlayerAgent player;

		[Inject]
		private readonly GoalMarkerController goalMarker;

		[Inject]
		private readonly ObjectiveManager objectiveManager;

		#endregion Private Fields

		#region Public Events

		public event Action<bool> OnGamePaused;
		public event Action<bool> OnGameOver;
		public event Action OnReachedGoal;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool IsGamePaused { get; private set; }

		[field: SerializeField, ReadOnly]
		public bool IsGameOver { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void SetIsGamePaused(bool isPaused)
		{
			IsGamePaused = isPaused;
			OnGamePaused?.Invoke(IsGamePaused);
			Time.timeScale = isPaused ? 0 : 1;
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			player.OnDeath += InvokeGameOver;
			objectiveManager.OnNoDetectionFailed += InvokeGameOver;
			goalMarker.OnReachedGoal += GoalMarker_OnReachedGoal;
		}

		private void OnDestroy()
		{
			player.OnDeath -= InvokeGameOver;
			objectiveManager.OnNoDetectionFailed -= InvokeGameOver;
			goalMarker.OnReachedGoal -= GoalMarker_OnReachedGoal;
		}

		private void InvokeGameOver()
		{
			if (IsGameOver)
				return;

			SetIsGameOver(true);
		}

		private void SetIsGameOver(bool value)
		{
			SetIsGamePaused(false);
			IsGameOver = value;
			OnGameOver?.Invoke(IsGameOver);
		}

		private void GoalMarker_OnReachedGoal()
		{
			Time.timeScale = 0;
			OnReachedGoal?.Invoke();
			levelManager.OnLevelComplete();
			IsGameOver = true;
			IsGamePaused = true;
		}

		#endregion Private Methods
	}
}