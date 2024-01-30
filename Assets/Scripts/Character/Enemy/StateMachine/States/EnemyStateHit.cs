using DG.Tweening;
using StealthTD.Extensions;
using System.Collections;
using UnityEngine;

namespace StealthTD.Enemy.States
{
	public class EnemyStateHit : EnemyState
	{
		#region Private Fields

		[SerializeField, ReadOnly]
		private bool wasPlayerCloseOnEnter;

		[SerializeField, ReadOnly]
		private float playerCloseTimer;

		[SerializeField, ReadOnly]
		private Vector3 hitNormal;

		[SerializeField, ReadOnly]
		private Vector3 hitPosition;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField, ReadOnly]
		public bool DidStateTimeOut { get; private set; }

		[field: SerializeField, ReadOnly]
		public bool IsPlayerDetected { get; private set; }

		[field: SerializeField, ReadOnly]
		public EnemyState PreviousState { get; private set; }

		#endregion Public Properties

		#region Private Properties

		private string LookTweenId => $"{transform.gameObject.GetInstanceID()}_EnemyHit";

		#endregion Private Properties

		#region Public Methods

		public void SetStateParameters(Vector3 hitPosition, Vector3 hitNormal, EnemyState previousState)
		{
			this.hitPosition = hitPosition;
			this.hitNormal = hitNormal;
			PreviousState = previousState;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void Enter()
		{
			vision.StopVision();
			wasPlayerCloseOnEnter = moveController.DistanceToPlayer <= 1.5f;
			DidStateTimeOut = false;
			playerCloseTimer = 0;

			if (wasPlayerCloseOnEnter && !localData.IsBeingPunched)
				moveController.WalkTowardsDestination(transform.position - hitNormal);
			else
				moveController.StopMoving();

			animator.SetFloat("MoveVelocityMagnitude", 0);
			StartCoroutine(HitRoutine());
		}

		protected override void Exit()
		{
			StopAllCoroutines();
			DOTween.Kill(LookTweenId);
			vision.StartVision();
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			if (wasPlayerCloseOnEnter)
			{
				playerCloseTimer += Time.deltaTime;
				moveController.LookAtPosition(playerTransform.position);
			}

			IsPlayerDetected = playerCloseTimer > 0.3f;
		}

		private IEnumerator HitRoutine()
		{
			yield return animator.PlayAndWaitForStateInBothLayers("TakeDamage", 0.1f, 0.7f);
			animator.PlayStateInBothLayers("IdleMove", 0.1f);
			DOTween.Kill(LookTweenId);

			if (!wasPlayerCloseOnEnter)
			{
				Vector3 lookAtPos = hitPosition + hitNormal;
				lookAtPos.y = transform.position.y;
				yield return transform.DOLookAt(lookAtPos, 0.3f).SetId(LookTweenId).WaitForCompletion();
			}

			DidStateTimeOut = true;
		}

		#endregion Private Methods
	}
}