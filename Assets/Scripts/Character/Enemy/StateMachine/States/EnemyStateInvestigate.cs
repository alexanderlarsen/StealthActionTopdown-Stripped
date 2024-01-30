using StealthTD.Extensions;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StealthTD.Enemy.States
{
	public class EnemyStateInvestigate : EnemyState
	{
		#region Private Fields

		[SerializeField]
		private int stopInvestigationAtRemainingDistance = 1;

		[SerializeField]
		private int stopInvestigationWhenHasStoodStillForSeconds = 1;

		[SerializeField]
		private float tryWakeUpRadius = 2f;

		[SerializeField]
		private float bodyInvestigationEndDelay = 1f;

		[SerializeField]
		private float regularInvestigationEndDelayMinimum = 2.5f;

		[SerializeField]
		private float regularInvestigationEndDelayMaximum = 4f;

		[SerializeField]
		private LayerMask wakeUpLayerMask;

		[SerializeField, ReadOnly]
		private bool didTransitionToSelf;

		[SerializeField, ReadOnly]
		private bool isAlerted;

		[SerializeField, ReadOnly]
		private float hasNotMovedTimer;

		[SerializeField, ReadOnly]
		private Vector3 investigationPosition;

		private Vector3 positionLastFrame;

		private bool canReachInvestigationPosition;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool IsInvestigatingBody { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void SetStateParameters(
			Vector3 investigationPosition,
			bool didTransitionToSelf,
			bool isInvestigatingBody,
			bool isAlerted)
		{
			this.investigationPosition = investigationPosition;
			this.didTransitionToSelf = didTransitionToSelf;
			IsInvestigatingBody = isInvestigatingBody;
			this.isAlerted = isAlerted;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void Enter()
		{
			positionLastFrame = transform.position;
			hasNotMovedTimer = 0;
			localData.IsInvestigating = true;
			canReachInvestigationPosition = moveController.IsValidPosition(investigationPosition);

			if (isAlerted)
				audioManager.CharacterAudio.PlayAlertedAudio(transform.position, transform);
			else if (!didTransitionToSelf)
				audioManager.CharacterAudio.PlaySuspiciousAudio(transform.position, transform);

			animator.PlayState("IdleMove");
			StartCoroutine(InvestigateRoutine());
		}

		protected override void Exit()
		{
			localData.IsInvestigating = false;
			StopAllCoroutines();
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			if (!canReachInvestigationPosition || Vector3.Distance(transform.position, investigationPosition) <= 2)
				moveController.LookAtPosition(investigationPosition);

			CalculateHasNotMovedTime();
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);
		}

		private void CalculateHasNotMovedTime()
		{
			if (transform.position == positionLastFrame)
				hasNotMovedTimer += Time.deltaTime;

			positionLastFrame = transform.position;
		}

		private IEnumerator InvestigateRoutine()
		{
			moveController.SetStoppingDistance(1f);

			if (isAlerted)
				moveController.RunTowardsDestination(investigationPosition);
			else
				moveController.WalkTowardsDestination(investigationPosition);

			yield return new WaitUntil(() =>
				moveController.RemainingDistance <= stopInvestigationAtRemainingDistance
				|| hasNotMovedTimer >= stopInvestigationWhenHasStoodStillForSeconds);

			if (IsInvestigatingBody)
			{
				yield return new WaitForSeconds(1f);
				yield return animator.PlayAndWaitForState("Investigate Ground");
				TryWakeUpEnemiesInRange();
				yield return new WaitForSeconds(bodyInvestigationEndDelay);
			}
			else yield return new WaitForSeconds(Random.Range(regularInvestigationEndDelayMinimum, regularInvestigationEndDelayMaximum));

			localData.IsInvestigating = false;
		}

		private void TryWakeUpEnemiesInRange()
		{
			Collider[] colliders = new Collider[5];
			int numEnemiesInRange = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * tryWakeUpRadius, colliders, Quaternion.identity, wakeUpLayerMask);

			for (int i = 0; i < numEnemiesInRange; i++)
				if (colliders[i].TryGetComponent(out EnemyAgent enemy))
					enemy.TryWakeUp();
		}

		#endregion Private Methods
	}
}