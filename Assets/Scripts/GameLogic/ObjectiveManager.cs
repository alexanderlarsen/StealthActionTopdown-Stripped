using StealthTD.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace StealthTD.GameLogic
{
	public class ObjectiveManager : MonoBehaviour
	{
		#region Private Fields

		private bool hasNoDetectedFailedEventBeenInvoked;
		private List<EnemyAgent> enemies;
		private List<Keycard> keycards;

		#endregion Private Fields

		#region Public Events

		public event Action OnAllObjectivesCompleted;
		public event Action OnObjectivesUpdated;
		public event Action OnNoDetectionFailed;

		#endregion Public Events

		#region Public Properties

		public int TotalKeycardsCount => keycards.Count;
		public int TotalEnemiesCount => enemies.Count;

		[field: SerializeField, ReadOnly]
		public bool AllObjectivesCompleted { get; private set; }

		[field: SerializeField]
		public Objective GetAllKeycards { get; private set; }

		[field: SerializeField]
		public Objective KillAllEnemies { get; private set; }

		[field: SerializeField]
		public Objective NoDetection { get; private set; }

		[field: SerializeField, ReadOnly]
		public int CurrentKeycardsCount { get; private set; }

		[field: SerializeField, ReadOnly]
		public int CurrentEnemiesCount { get; private set; }

		public bool WasDetected { get; private set; }

		#endregion Public Properties

		#region Private Methods

		private void Start()
		{
			DiContainer diContainer = ProjectContext
				.Instance
				.Container
				.Resolve<SceneContextRegistry>()
				.GetContainerForScene(SceneManager.GetActiveScene());

			enemies = diContainer.ResolveAll<EnemyAgent>();
			keycards = diContainer.ResolveAll<Keycard>();

			enemies.ForEach(enemy =>
			{
				enemy.OnDeath += Enemy_OnDeath;
				enemy.OnPlayerDetected += Enemy_OnPlayerDetected;
			});

			keycards.ForEach(keycard => keycard.OnPickUp += Keycard_OnPickUp);

			CheckObjectives();
		}

		private void OnDestroy()
		{
			enemies.ForEach(enemy =>
			{
				enemy.OnDeath -= Enemy_OnDeath;
				enemy.OnPlayerDetected -= Enemy_OnPlayerDetected;
			});

			keycards.ForEach(keycard => keycard.OnPickUp -= Keycard_OnPickUp);
		}

		private void CheckObjectives()
		{
			AllObjectivesCompleted = GetAllKeycards.IsCompleted && KillAllEnemies.IsCompleted;

			if (AllObjectivesCompleted)
			{
				OnAllObjectivesCompleted?.Invoke();
				Debug.Log("All objectives completed");
			}

			OnObjectivesUpdated?.Invoke();
		}

		private void Enemy_OnDeath()
		{
			CurrentEnemiesCount++;

			if (KillAllEnemies.EnableObjective && CurrentEnemiesCount == TotalEnemiesCount)
				KillAllEnemies.MarkObjectiveCompleted();

			CheckObjectives();
		}

		private void Enemy_OnPlayerDetected()
		{
			if (NoDetection.EnableObjective && !hasNoDetectedFailedEventBeenInvoked)
			{
				WasDetected = true;
				OnNoDetectionFailed?.Invoke();
				hasNoDetectedFailedEventBeenInvoked = true;
			}

			CheckObjectives();
		}

		private void Keycard_OnPickUp()
		{
			CurrentKeycardsCount++;

			if (GetAllKeycards.EnableObjective && CurrentKeycardsCount == TotalKeycardsCount)
				GetAllKeycards.MarkObjectiveCompleted();

			CheckObjectives();
		}

		#endregion Private Methods

		#region Public Classes

		[Serializable]
		public class Objective
		{
			#region Private Fields

			[SerializeField, ReadOnly]
			private bool isCompleted;

			#endregion Private Fields

			#region Public Properties

			[field: SerializeField]
			public bool EnableObjective { get; private set; }

			public bool IsCompleted => !EnableObjective || isCompleted;

			#endregion Public Properties

			#region Public Methods

			public void MarkObjectiveCompleted()
			{
				isCompleted = true;
			}

			#endregion Public Methods
		}

		#endregion Public Classes
	}
}