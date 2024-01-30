using StealthTD.Extensions;
using StealthTD.Helpers;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.States
{
	public class EnemyStateDeath : EnemyState
	{
		#region Private Fields

		[Inject]
		private readonly Disposer disposer;

		#endregion Private Fields

		#region Protected Methods

		protected override void Enter()
		{
			vision.StopVision();
			moveController.StopMoving();
			weaponController.StopUsingCurrentWeapon();
			audioManager.CharacterAudio.PlayDeathAudio(transform.position, transform);
			bool isAlreadyOnGround = animator.IsInState("Overhead Knockout");

			if (isAlreadyOnGround)
			{
				OnHitGround();
				return;
			}

			animator.PlayStateInBothLayers("Death");
			eventRelayer.Subscribe("OnHitGround", OnHitGround);
		}

		protected override void Exit()
		{
			eventRelayer.Unsubscribe("OnHitGround", OnHitGround);
		}

		#endregion Protected Methods

		#region Private Methods

		private void OnHitGround()
		{
			audioManager.CharacterAudio.PlayBodyDropAudio(transform.position, transform);
			vfxManager.DisplayBloodPoolVfx(transform);
			weaponController.UnequipWeapon();
			animator.SetActiveAnimatorLayer(AnimatorLayer.Unarmed);
			transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			disposer.DisposeAll();
		}

		#endregion Private Methods
	}
}