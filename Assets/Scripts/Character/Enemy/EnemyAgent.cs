using StealthTD.Audio;
using StealthTD.Character;
using StealthTD.Enemy.Data;
using StealthTD.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy
{
	public class EnemyAgent :
		MonoBehaviour,
		ILandingResponder,
		IDamagable,
		IVisionDetectable,
		IHitable,
		IPunchable,
		ISurface
	{
		#region Private Fields

		[Inject]
		private readonly EnemyLocalData localData;

		[Inject]
		private readonly AudioManager audioManager;

		[Inject]
		private readonly CharacterBoneReferences boneReferences;

		[SerializeField, ReadOnly]
		private float punchTimer;

		private bool deadBodyHasBeenDiscovered;
		private bool hasPlayerDetectedEventBeenInvoked;

		#endregion Private Fields

		#region Public Events

		public event Action<IHitable.HitEventArgs> OnHit;
		public event Action OnDeath;

		public event Action OnPlayerDetected;

		#endregion Public Events

		#region Public Properties

		public bool IsDead => localData.Health <= 0;
		public SurfaceType Type => SurfaceType.Body;
		public Transform HeadTransform => boneReferences.Head;
		public Transform Transform => transform;

		public bool IsDetectable => localData.Health <= 0
			&& !deadBodyHasBeenDiscovered
			|| localData.Health > 0
			&& localData.UnconsciousTimeLeft > 0;

		#endregion Public Properties

		#region Public Methods

		public void OnPlayerLand()
		{
			localData.UnconsciousTimeLeft = 10;
		}

		public void TakeDamage(int damage)
		{
			if (localData.Health <= 0)
				return;

			localData.Health -= damage;

			if (localData.Health <= 0)
			{
				OnDeath?.Invoke();
			}
		}

		public void Hit(Vector3 hitPosition, Vector3 hitNormal)
		{
			if (localData.IsDead)
				return;

			audioManager.CharacterAudio.PlayHitAudio(transform.position, transform);

			if (localData.IsUnconscious)
				return;

			OnHit?.Invoke(new(hitPosition, hitNormal));
		}

		public void TakePunch()
		{
			if (localData.IsUnconscious)
			{
				localData.PunchCount = 0;
				punchTimer = 0;
				return;
			}

			localData.PunchCount++;
			punchTimer = 1f;
			audioManager.CharacterAudio.PlayHitAudio(transform.position, transform);
			OnHit?.Invoke(new(transform.position, -transform.forward));

			if (localData.PunchCount > 2)
				localData.UnconsciousTimeLeft = 10;
		}

		public bool TryWakeUp()
		{
			if (localData.IsDead)
			{
				deadBodyHasBeenDiscovered = true;
				return false;
			}

			localData.UnconsciousTimeLeft = 0;
			return true;
		}

		public void TryMarkDeadBodyAsDiscovered()
		{
			if (localData.IsDead)
				deadBodyHasBeenDiscovered = true;
		}

		#endregion Public Methods

		#region Private Methods

		private void Update()
		{
			if (punchTimer > 0)
			{
				punchTimer -= Time.deltaTime;

				if (punchTimer <= 0)
					localData.PunchCount = 0;
			}

			if (localData.IsPlayerDetected && !hasPlayerDetectedEventBeenInvoked)
			{
				OnPlayerDetected?.Invoke();
				hasPlayerDetectedEventBeenInvoked = true;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (localData.IsDead
				|| localData.IsUnconscious
				|| localData.IsBeingPunched)
				return;

			if (other.CompareTag("Player"))
				OnHit?.Invoke(new(transform.position, -transform.forward));
		}

		[ContextMenu("TriggerUnconsciousness")]
		private void TriggerUnconsciousness()
		{
			if (!Application.isPlaying)
				return;

			localData.UnconsciousTimeLeft = 10;
		}

		[ContextMenu("TriggerDeath")]
		private void TriggerDeath()
		{
			if (!Application.isPlaying)
				return;

			localData.Health = 0;
		}

		#endregion Private Methods
	}
}