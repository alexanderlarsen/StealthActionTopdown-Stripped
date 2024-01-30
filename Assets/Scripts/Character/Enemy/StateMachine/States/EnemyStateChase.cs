using StealthTD.Extensions;
using StealthTD.Weapons;
using System.Collections;
using UnityEngine;

namespace StealthTD.Enemy.States
{
	public class EnemyStateChase : EnemyState
	{
		#region Private Fields

		[SerializeField]
		private float initialAttackDelay = 0.3f;

		[SerializeField,]
		private float stoppingDistanceOnEnter = 0.5f;

		[SerializeField]
		private float loseTargetThreshold = 6;

		[SerializeField]
		private float requestBackupThreshold = 1.25f;

		[SerializeField]
		private float chaseTimeAfterLostSight = 2.5f;

		[SerializeField]
		private LayerMask lineOfFireLayerMask;

		[SerializeField, ReadOnly]
		private bool hasLineOfFire;

		[SerializeField, ReadOnly]
		private bool isPlayerVisible;

		[SerializeField, ReadOnly]
		private bool isShooting;

		[SerializeField, ReadOnly]
		private bool requestBackup;

		[SerializeField, ReadOnly]
		private float targetLostTimer;

		[SerializeField, ReadOnly]
		private bool hasLostTargetDebug;

		[SerializeField, ReadOnly]
		private float timeSinceEnterStateDebug;

		private float enterStateTime;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool WasBackupRequestInterfered { get; private set; }

		public bool HasLostTarget => hasLostTargetDebug = targetLostTimer >= loseTargetThreshold;

		#endregion Public Properties

		#region Private Properties

		private float TimeSinceEnterState => timeSinceEnterStateDebug = Time.time - enterStateTime;

		#endregion Private Properties

		#region Public Methods

		public void SetStateParameters(bool requestBackup)
		{
			this.requestBackup = requestBackup;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void Enter()
		{
			enterStateTime = Time.time;
			targetLostTimer = 0;
			WasBackupRequestInterfered = false;
			moveController.SetStoppingDistance(stoppingDistanceOnEnter);
			animator.PlayState("IdleMove");

			if (requestBackup)
			{
				StartCoroutine(RequestBackupRoutine());
				audioManager.CharacterAudio.PlayAlertedAudio(transform.position, transform);
			}
		}

		protected override void Exit()
		{
			if (!requestBackup && localData.Health > 0)
				sharedData.CancelBackup(enemy);

			localData.RequestBackupTimer = 0;
			StopAllCoroutines();
			StopShooting();
			weaponController.CancelReload();
			weaponController.ResetWeaponRotation();
			moveController.SetAngularSpeed(500);
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			isPlayerVisible = localData.PlayerDetectedPercentage >= 1;
			hasLineOfFire = weaponController.CurrentWeapon is ShootingWeapon shootingWeapon
				&& !Physics.Linecast(shootingWeapon.MuzzleOut.position, playerTransform.position, lineOfFireLayerMask);

			if (moveController.DistanceToPlayer <= 2)
				isPlayerVisible = true;

			if (targetLostTimer < chaseTimeAfterLostSight)
				sharedData.SetLastKnownPlayerPosition();

			HandleWeapon();
			CalculateTargetLostTimer();
			HandleMovement();
			SetAnimatorParameters();
		}

		private void HandleMovement()
		{
			Vector3 destination = moveController.FindBestAttackPosition(isPlayerVisible, hasLineOfFire, lineOfFireLayerMask);
			moveController.RunTowardsDestination(destination);
			moveController.SetAngularSpeed(isPlayerVisible ? 0 : 800);

			if (isPlayerVisible)
				moveController.LookAtPosition(playerTransform.position);
		}

		private void CalculateTargetLostTimer()
		{
			if (isPlayerVisible)
				targetLostTimer = 0;
			else
				targetLostTimer += Time.deltaTime;
		}

		private void HandleWeapon()
		{
			if (TimeSinceEnterState <= initialAttackDelay)
				return;

			weaponController.HandleWeaponRotation(isPlayerVisible, playerTransform.position);

			if (weaponController.ShouldReload)
			{
				StopShooting();
				weaponController.ReloadCurrentWeapon("IdleMove");
			}

			bool canShoot = isPlayerVisible && hasLineOfFire && !weaponController.IsReloading;

			if (canShoot && !isShooting)
				StartShooting();
			else if (!canShoot && isShooting)
				StopShooting();
		}

		private void StartShooting()
		{
			isShooting = true;
			weaponController.UseCurrentWeapon();
		}

		private void StopShooting()
		{
			isShooting = false;
			weaponController.StopUsingCurrentWeapon();
		}

		private void SetAnimatorParameters()
		{
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);
		}

		private IEnumerator RequestBackupRoutine()
		{
			WasBackupRequestInterfered = true;
			localData.RequestBackupTimer = 0;
			localData.RequestBackupThreshold = requestBackupThreshold;

			while (localData.RequestBackupTimer < localData.RequestBackupThreshold)
			{
				localData.RequestBackupTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			sharedData.RequestBackup(enemy);
			localData.RequestBackupTimer = 0;
			WasBackupRequestInterfered = false;
		}

		#endregion Private Methods
	}
}