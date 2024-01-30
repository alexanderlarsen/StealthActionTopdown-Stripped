using DG.Tweening;
using StealthTD.Extensions;
using System.Collections;
using UnityEngine;

namespace StealthTD.Enemy.States
{
	public class EnemyStateUnconscious : EnemyState
	{
		#region Protected Methods

		protected override void Enter()
		{
			StartCoroutine(UnconsciousRoutine());
		}

		protected override void Exit()
		{
			StopAllCoroutines();
			string id = $"{transform.gameObject.GetInstanceID()}_EnemyStateUnconscious_Exit";
			DOTween.Kill(id);
			DOVirtual.Float(localData.UnconsciousTimeLeft, 0, 0.1f, val => localData.UnconsciousTimeLeft = val).SetId(id);
			animator.TweenLayerWeight(1, 0, 0.2f);
			vision.StartVision();
		}

		#endregion Protected Methods

		#region Private Methods

		private IEnumerator UnconsciousRoutine()
		{
			localData.ResetHit();
			vision.StopVision();
			moveController.StopMoving();
			moveController.LookInRandomDirection();
			animator.PlayState("Overhead Knockout", 0.25f, 0);
			animator.PlayState("Death", 0.25f, 1);
			animator.TweenLayerWeight(1, 0, 0.2f);
			audioManager.CharacterAudio.PlayOverheadKnockOutAudio(transform.position, transform);
			localData.UnconsciousTimeLeft = localData.UnconsciousDuration;
			YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();

			while (localData.UnconsciousTimeLeft > 0f)
			{
				localData.UnconsciousTimeLeft -= Time.deltaTime;
				yield return waitForEndOfFrame;
			}

			yield return animator.PlayAndWaitForState("Get Up", 0.25f, 0.95f);
		}

		#endregion Private Methods
	}
}