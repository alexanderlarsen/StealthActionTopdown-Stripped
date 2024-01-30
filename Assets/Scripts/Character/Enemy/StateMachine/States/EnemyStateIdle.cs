using StealthTD.Extensions;
using System.Collections;
using UnityEngine;

namespace StealthTD.Enemy.States
{
	public class EnemyStateIdle : EnemyState
	{
		#region Private Fields

		[SerializeField]
		private int rotationSpeed = 350;

		[SerializeField]
		private float stoppingDistanceBuffer = 0.1f;

		private Quaternion idleRotation;
		private Vector3 idlePosition;

		#endregion Private Fields

		#region Protected Methods

		protected override void Enter()
		{
			animator.PlayStateInBothLayers("IdleMove");
			StartCoroutine(ReturnToIdleRoutine());
		}

		protected override void Exit()
		{
			StopAllCoroutines();
		}

		#endregion Protected Methods

		#region Private Methods

		private void Awake()
		{
			idlePosition = transform.position;
			idleRotation = transform.rotation;
		}

		private void Update()
		{
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);
		}

		private IEnumerator ReturnToIdleRoutine()
		{
			yield return new WaitForSeconds(1);

			moveController.SetStoppingDistance(0);
			moveController.WalkTowardsDestination(idlePosition);

			while (transform.rotation != idleRotation)
			{
				if (moveController.RemainingDistance <= moveController.StoppingDistance + stoppingDistanceBuffer)
					transform.rotation = Quaternion.RotateTowards(transform.transform.rotation, idleRotation, rotationSpeed * Time.deltaTime);

				yield return new WaitForEndOfFrame();
			}
		}

		#endregion Private Methods
	}
}