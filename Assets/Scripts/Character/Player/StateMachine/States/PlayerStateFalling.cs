using StealthTD.Extensions;
using StealthTD.Interfaces;
using System.Collections;
using UnityEngine;

namespace StealthTD.Player.States
{
	public class PlayerStateFalling : PlayerState
	{
		#region Private Fields

		private const float rotationSpeed = 350;

		private Coroutine fallingRoutine;
		private float moveSpeed = 1;

		#endregion Private Fields

		#region Public Properties

		public bool IsFallComplete { get; private set; }

		#endregion Public Properties

		#region Protected Methods

		protected override void Enter()
		{
			IsFallComplete = false;
			fallingRoutine = StartCoroutine(FallingRoutine());
		}

		protected override void Exit()
		{
			StopCoroutine(fallingRoutine);
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			if (IsPaused)
				return;

			moveController.Move(input.MoveDirection, moveSpeed);
			moveController.Rotate(rotationSpeed);
			weaponController.HandleWeaponRotation();

			//if (moveController.IsGrounded)
			//	return;

			if (input.Attack.WasPressedThisFrame())
			{
				if (weaponController.HasWeapon)
					weaponController.UseCurrentWeapon();
				else
					punchController.Attack();
			}
			else if (input.Reload.WasPressedThisFrame())
				weaponController.ReloadCurrentWeapon();
			else if (input.Attack.WasReleasedThisFrame() && weaponController.HasWeapon)
				weaponController.StopUsingCurrentWeapon();
			else if (input.DropItem.WasPressedThisFrame())
				equipManager.UnequipCurrentItem();
			else if (input.Interact.WasPressedThisFrame())
				equipManager.EquipClosestItem();
		}

		private IEnumerator FallingRoutine()
		{
			animator.PlayStateInBothLayers("Fall");
			yield return new WaitUntil(() => moveController.IsGrounded);
			FindAndTriggerLandingResponses();
			moveSpeed = 0;
			yield return animator.PlayAndWaitForStateInBothLayers("LandFromFall");
			IsFallComplete = true;
		}

		private void FindAndTriggerLandingResponses()
		{
			Collider[] colliders = new Collider[5];
			int overlapLength = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * 0.3f, colliders, Quaternion.identity, LayerMask.GetMask("Enemy"));

			for (int i = 0; i < overlapLength; i++)
				if (colliders[i].TryGetComponent(out ILandingResponder landingResponse))
					landingResponse.OnPlayerLand();
		}

		#endregion Private Methods
	}
}