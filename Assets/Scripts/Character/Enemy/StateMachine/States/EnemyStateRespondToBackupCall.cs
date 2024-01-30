using UnityEngine;

namespace StealthTD.Enemy.States
{
	public class EnemyStateRespondToBackupCall : EnemyState
	{
		#region Private Fields

		[SerializeField, ReadOnly]
		private float stoodStillTimer;

		private Transform backupRequesterTransform;
		private Vector3 positionAtLastFrame;

		#endregion Private Fields

		#region Public Properties

		public bool DidStateTimeOut => stoodStillTimer >= 2f;
		public bool DidReachRequester => Vector3.Distance(transform.position, backupRequesterTransform.position) <= 3f;

		#endregion Public Properties

		#region Public Methods

		public void SetStateParameters(Transform backupRequesterTransform)
		{
			this.backupRequesterTransform = backupRequesterTransform;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void Enter()
		{
			stoodStillTimer = 0;
			positionAtLastFrame = transform.position;
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			if (IsPaused)
				return;

			moveController.RunTowardsDestination(backupRequesterTransform.position);
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);

			if (positionAtLastFrame == transform.position)
				stoodStillTimer += Time.deltaTime;

			positionAtLastFrame = transform.position;
		}

		#endregion Private Methods
	}
}