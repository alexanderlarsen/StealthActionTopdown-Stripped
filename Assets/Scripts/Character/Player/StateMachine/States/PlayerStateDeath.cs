using StealthTD.Extensions;
using UnityEngine;

namespace StealthTD.Player.States
{
	public class PlayerStateDeath : PlayerState
	{
		#region Protected Methods

		protected override void Enter()
		{
			StopAllCoroutines();
			weaponController.StopUsingCurrentWeapon();
			audioManager.CharacterAudio.PlayDeathAudio(transform.position, transform);
			eventRelayer.Subscribe("OnHitGround", AnimationEvent_OnHitGround);
			animator.PlayStateInBothLayers("Death");
		}

		protected override void Exit()
		{
			eventRelayer.Unsubscribe("OnHitGround", AnimationEvent_OnHitGround);
		}

		#endregion Protected Methods

		#region Private Methods

		private void AnimationEvent_OnHitGround()
		{
			audioManager.CharacterAudio.PlayBodyDropAudio(transform.position, transform);
			transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			weaponController.UnequipWeapon(true);
			vfxManager.DisplayBloodPoolVfx(transform);
		}

		#endregion Private Methods
	}
}