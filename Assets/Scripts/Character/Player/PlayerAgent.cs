using StealthTD.Interfaces;
using StealthTD.Player.Data;
using System;
using UnityEngine;
using Zenject;

namespace StealthTD.Player
{
	public class PlayerAgent :
		MonoBehaviour,
		IVisionDetectable,
		IDamagable,
		ISurface
	{
		#region Private Fields

		[Inject]
		private readonly PlayerLocalData localData;

		[SerializeField]
		private bool isInvisible;

		[SerializeField]
		private bool isInvincible;

		#endregion Private Fields

		#region Public Events

		public event Action OnDeath;

		#endregion Public Events

		#region Public Properties

		public bool IsDead => localData.Health <= 0;
		public bool IsDetectable => !isInvisible && !IsDead;
		public SurfaceType Type => SurfaceType.Body;
		public Transform Transform => transform;

		#endregion Public Properties

		#region Public Methods

		public void TakeDamage(int damage)
		{
			if (isInvincible)
				return;

			if (localData.Health <= 0)
				return;

			localData.Health -= damage;

			if (localData.Health <= 0)
				OnDeath?.Invoke();
		}

		#endregion Public Methods

		#region Private Methods

		[ContextMenu(nameof(Kill))]
		private void Kill()
		{
			if (!Application.isPlaying)
				return;

			TakeDamage(100);
		}

		#endregion Private Methods
	}
}