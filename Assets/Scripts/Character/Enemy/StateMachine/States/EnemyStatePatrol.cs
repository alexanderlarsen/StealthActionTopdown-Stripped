using StealthTD.Extensions;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StealthTD.Enemy.States
{
	public class EnemyStatePatrol : EnemyState
	{
		#region Private Fields

		[SerializeField]
		private float stoppingDistance = 0.75f;

		[SerializeField]
		private float nextPatrolPointDelayMin = 2f;

		[SerializeField]
		private float nextPatrolPointDelayMax = 3f;

		private Coroutine patrolRoutine;

		#endregion Private Fields

		#region Protected Methods

		protected override void Enter()
		{
			moveController.SetStoppingDistance(stoppingDistance);
			animator.PlayStateInBothLayers("IdleMove");
			patrolRoutine = StartCoroutine(PatrolRoutine());
		}

		protected override void Exit()
		{
			StopCoroutine(patrolRoutine);
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);
		}

		private IEnumerator PatrolRoutine()
		{
			int index = patrolPath.FindNearestPatrolPointIndex(transform.position);
			YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();

			while (true)
			{
				Vector3 currentDestination = patrolPath.GetPoint(index);

				while (Vector3.Distance(transform.position, currentDestination) > moveController.StoppingDistance + 0.1f)
				{
					moveController.WalkTowardsDestination(currentDestination);
					yield return waitForEndOfFrame;
				}

				if (index == 0 || index == patrolPath.NumPoints - 1)
					yield return new WaitForSeconds(Random.Range(nextPatrolPointDelayMin, nextPatrolPointDelayMax));

				index = (index + 1) % patrolPath.NumPoints;
			}
		}

		#endregion Private Methods
	}
}