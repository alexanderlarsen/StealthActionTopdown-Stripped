using StealthTD.Player;
using System;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy
{
	/// <summary>
	/// Singleton class shared between all enemies.
	/// </summary>
	public class EnemySharedData : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly PlayerAgent player;

		[SerializeField, ReadOnly]
		private EnemyAgent originalRequester;

		[SerializeField, ReadOnly]
		private bool isPlayerDeadDebug;

		#endregion Private Fields

		#region Public Events

		public event Action<EnemyAgent> OnEnemyRequestBackup;
		public event Action<EnemyAgent> OnEnemyCancelBackup;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public Vector3 LastKnownPlayerPosition { get; private set; }

		public bool IsPlayerDead => isPlayerDeadDebug = player.IsDead;

		#endregion Public Properties

		#region Public Methods

		public void RequestBackup(EnemyAgent requester)
		{
			originalRequester = requester;
			OnEnemyRequestBackup?.Invoke(requester);
		}

		public void CancelBackup(EnemyAgent requester)
		{
			if (originalRequester == null)
				return;

			if (requester != originalRequester)
				return;

			originalRequester = null;
			OnEnemyCancelBackup?.Invoke(requester);
		}

		public void SetLastKnownPlayerPosition()
		{
			LastKnownPlayerPosition = player.transform.position;
		}

		#endregion Public Methods
	}
}