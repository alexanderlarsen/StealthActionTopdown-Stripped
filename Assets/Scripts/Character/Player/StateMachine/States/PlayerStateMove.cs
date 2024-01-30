using StealthTD.Extensions;

namespace StealthTD.Player.States
{
	public class PlayerStateMove : PlayerState
	{
		#region Private Fields

		private readonly float walkSpeed = 2;
		private readonly float runSpeed = 5;
		private readonly float rotationSpeed = 350;

		#endregion Private Fields

		#region Protected Methods

		protected override void Enter()
		{
			animator.PlayStateInBothLayers("IdleMove");
		}

		#endregion Protected Methods

		#region Private Methods

		private void Update()
		{
			if (IsPaused)
				return;

			float moveSpeed = input.Sneak.IsPressed() ? walkSpeed : runSpeed;
			moveController.Move(input.MoveDirection, moveSpeed);
			moveController.Rotate(rotationSpeed);
			weaponController.HandleWeaponRotation();
			animator.SetFloat("MoveVelocityMagnitude", moveController.Velocity.magnitude);

			if (input.Attack.WasPressedThisFrame())
			{
				if (weaponController.HasWeapon)
					weaponController.UseCurrentWeapon("IdleMove");
				else
					punchController.Attack();
			}
			else if (input.Reload.WasPressedThisFrame())
				weaponController.ReloadCurrentWeapon("IdleMove");
			else if (input.Attack.WasReleasedThisFrame() && weaponController.HasWeapon)
				weaponController.StopUsingCurrentWeapon();
			else if (input.DropItem.WasPressedThisFrame())
				equipManager.UnequipCurrentItem();
			else if (input.Interact.WasPressedThisFrame())
				equipManager.EquipClosestItem();
		}

		#endregion Private Methods
	}
}