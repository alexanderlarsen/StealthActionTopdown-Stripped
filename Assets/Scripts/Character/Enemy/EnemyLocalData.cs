using StealthTD.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.Data
{
	/// <summary>
	/// Local data pertaining to one enemy.
	/// </summary>
	public class EnemyLocalData : MonoBehaviour, INotifyPropertyChanged
	{
		#region Private Fields

		[Inject]
		private readonly IHitable hitable;

		[Inject]
		private readonly EnemySharedData sharedData;

		[SerializeField, ReadOnly]
		private bool isInvestigating;

		[SerializeField, ReadOnly]
		private bool wasCalledForBackup;

		[SerializeField, ReadOnly]
		private bool wasHit;

		[SerializeField, ReadOnly]
		private float playerDetectedAmount = 0;

		[SerializeField, ReadOnly]
		private float requestBackupTimer;

		[SerializeField, ReadOnly]
		private float unconsciousTimeLeft = 0;

		[SerializeField, ReadOnly]
		private int health = 100;

		[SerializeField, ReadOnly]
		private Transform backupRequesterTransform;

		[SerializeField, ReadOnly]
		private Vector3 hitNormal;

		[SerializeField, ReadOnly]
		private Vector3 hitPosition;

		[SerializeField, ReadOnly]
		private bool isBeingPunchedDebug;

		[SerializeField, ReadOnly]
		private bool isDeadDebug;

		[SerializeField, ReadOnly]
		private bool isPlayerDetectedDebug;

		[SerializeField, ReadOnly]
		private bool isUnconsciousDebug;

		[SerializeField, ReadOnly]
		private float playerDetectedPercentageDebug;

		[SerializeField, ReadOnly]
		private float requestBackupPercentageDebug;

		[SerializeField, ReadOnly]
		private float unconsciousPercentageDebug;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public float RequestBackupThreshold { get; set; }

		[field: SerializeField, ReadOnly]
		public float UnconsciousDuration { get; set; } = 10;

		[field: SerializeField, ReadOnly]
		public float PlayerDetectionThreshold { get; set; } = 10;

		[field: SerializeField, ReadOnly]
		public int PunchCount { get; set; }

		public bool IsBeingPunched => isBeingPunchedDebug = PunchCount > 0;
		public bool IsDead => isDeadDebug = health <= 0;

		public bool IsPlayerDetected => isPlayerDetectedDebug = PlayerDetectedPercentage >= 1;

		public bool IsUnconscious => isUnconsciousDebug = unconsciousTimeLeft > 0;
		public float PlayerDetectedPercentage => playerDetectedPercentageDebug = PlayerDetectedAmount / PlayerDetectionThreshold;
		public float RequestBackupPercentage => requestBackupPercentageDebug = RequestBackupTimer / RequestBackupThreshold;
		public float UnconsciousPercentage => unconsciousPercentageDebug = UnconsciousTimeLeft / UnconsciousDuration;

		public bool IsInvestigating
		{
			get => isInvestigating;
			set
			{
				if (isInvestigating == value)
					return;

				isInvestigating = value;
				InvokePropertyChanged();
			}
		}

		public float PlayerDetectedAmount
		{
			get => playerDetectedAmount;
			set
			{
				if (playerDetectedAmount == value)
					return;

				playerDetectedAmount = value;
				InvokePropertyChanged();
			}
		}

		public float UnconsciousTimeLeft
		{
			get => unconsciousTimeLeft;
			set
			{
				if (unconsciousTimeLeft == value)
					return;

				unconsciousTimeLeft = value;
				InvokePropertyChanged();
			}
		}

		public float RequestBackupTimer
		{
			get => requestBackupTimer;
			set
			{
				if (requestBackupTimer == value)
					return;

				requestBackupTimer = value;
				InvokePropertyChanged();
			}
		}

		public int Health
		{
			get => health;
			set
			{
				if (health == value)
					return;

				health = value;
				InvokePropertyChanged();
			}
		}

		#endregion Public Properties

		#region Public Methods

		public bool WasCalledForBackup(out Transform backupRequesterTransform)
		{
			if (!wasCalledForBackup)
			{
				backupRequesterTransform = null;
				return false;
			}

			backupRequesterTransform = this.backupRequesterTransform;
			wasCalledForBackup = false;
			return true;
		}

		public bool WasHit(out Vector3 hitPosition, out Vector3 hitNormal)
		{
			if (!wasHit)
			{
				hitPosition = Vector3.negativeInfinity;
				hitNormal = Vector3.negativeInfinity;
				return false;
			}

			hitPosition = this.hitPosition;
			hitNormal = this.hitNormal;
			wasHit = false;
			return true;
		}

		public void ResetHit()
		{
			wasHit = false;
			hitPosition = Vector3.negativeInfinity;
			hitNormal = Vector3.negativeInfinity;
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			hitable.OnHit += Hitable_OnHit;
			sharedData.OnEnemyRequestBackup += SharedData_OnEnemyRequestBackup;
			sharedData.OnEnemyCancelBackup += SharedData_OnEnemyCancelBackup;
		}

		private void OnDestroy()
		{
			hitable.OnHit -= Hitable_OnHit;
			sharedData.OnEnemyRequestBackup -= SharedData_OnEnemyRequestBackup;
			sharedData.OnEnemyCancelBackup -= SharedData_OnEnemyCancelBackup;
		}

		private void SharedData_OnEnemyRequestBackup(EnemyAgent enemy)
		{
			backupRequesterTransform = enemy.transform;
			wasCalledForBackup = true;
		}

		private void SharedData_OnEnemyCancelBackup(EnemyAgent enemy)
		{
			wasCalledForBackup = false;
			backupRequesterTransform = null;
		}

		private void Hitable_OnHit(IHitable.HitEventArgs args)
		{
			hitPosition = args.hitPosition;
			hitNormal = args.hitNormal;
			wasHit = true;
		}

		private void InvokePropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion Private Methods
	}
}