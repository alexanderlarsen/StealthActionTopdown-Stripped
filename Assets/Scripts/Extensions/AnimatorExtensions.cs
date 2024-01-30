using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace StealthTD.Extensions
{
	public static class AnimatorExtensions
	{
		#region Public Methods

		public static void PlayState(this Animator animator, string stateName, float fixedTransitionDuration = 0.25f, int layer = 0)
		{
			animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer);
		}

		public static void PlayStateInBothLayers(this Animator animator, string stateName, float fixedTransitionDuration = 0.25f)
		{
			animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration, 0);
			animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration, 1);
		}

		public static void SetActiveAnimatorLayer<T>(this Animator animator, T layer) where T : System.Enum
		{
			if (layer.ToInt() < 2)
				throw new System.InvalidOperationException("Don't change layer weight of the two first Animator layers.");

			for (int i = 2; i < animator.layerCount; i++)
			{
				float layerWeight = i == layer.ToInt() ? 1 : 0;
				animator.TweenLayerWeight(i, layerWeight, 0.2f);
			}
		}

		public static bool IsInState(this Animator animator, string stateName, int layer = 0)
		{
			return animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName);
		}

		public static IEnumerator WaitUntilStateDone(this Animator animator, string stateName, float normalizedTimeThreshold = 0.9f, int layer = 0)
		{
			yield return new WaitUntil(() =>
				animator.IsInState(stateName, layer)
				&& animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > normalizedTimeThreshold);
		}

		public static IEnumerator WaitUntilIsInState(this Animator animator, string stateName, int layer = 0)
		{
			yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName));
		}

		public static IEnumerator PlayAndWaitForState(this Animator animator, string stateName, float fixedTransitionDuration = 0.25f, float normalizedTimeThreshold = 0.9f, int layer = 0)
		{
			animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer);
			yield return animator.WaitUntilStateDone(stateName, normalizedTimeThreshold, layer);
		}

		public static IEnumerator PlayAndWaitForStateInBothLayers(this Animator animator, string stateName, float fixedTransitionDuration = 0.25f, float normalizedTimeThreshold = 0.9f)
		{
			animator.PlayState(stateName, fixedTransitionDuration, 0);
			yield return animator.PlayAndWaitForState(stateName, fixedTransitionDuration, normalizedTimeThreshold, 1);
		}

		public static Tweener TweenLayerWeight(this Animator animator, int layer, float targetWeight, float duration)
		{
			string id = $"animator_{animator.gameObject.GetInstanceID()}_TweenLayerWeight_{layer}";
			DOTween.Kill(id);
			float currentLayerWeight = animator.GetLayerWeight(layer);

			if (currentLayerWeight == targetWeight)
				return null;

			return DOVirtual.Float(currentLayerWeight, targetWeight, duration, val => animator.SetLayerWeight(layer, val)).SetId(id);
		}

		#endregion Public Methods
	}
}