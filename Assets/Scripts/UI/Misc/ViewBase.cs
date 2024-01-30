using DG.Tweening;
using UnityEngine;

namespace StealthTD.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class ViewBase : MonoBehaviour
	{
		#region Protected Fields

		[SerializeField]
		protected CanvasGroup canvasGroup;

		#endregion Protected Fields

		#region Private Properties

		private string FadeId => $"{gameObject.GetInstanceID()}_MenuViewBase_Fade";

		#endregion Private Properties

		#region Public Methods

		protected Tweener FadeIn(float duration)
		{
			canvasGroup.alpha = 0;
			gameObject.SetActive(true);
			KillFadeTween();
			return canvasGroup.DOFade(1, duration).SetId(FadeId).SetUpdate(true);
		}

		protected Tweener FadeOut(float duration)
		{
			KillFadeTween();
			return canvasGroup.DOFade(0, duration).SetId(FadeId).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
		}

		public void Enable()
		{
			KillFadeTween();
			canvasGroup.alpha = 1;
			gameObject.SetActive(true);
		}

		public void Disable()
		{
			KillFadeTween();
			canvasGroup.alpha = 0;
			gameObject.SetActive(false);
		}

		#endregion Public Methods

		#region Protected Methods

		protected void SetVisibility(bool isVisible, bool fade, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
		{
			if (isVisible && fade)
				FadeIn(fadeInDuration);
			else if (!isVisible && fade)
				FadeOut(fadeOutDuration);
			else if (isVisible)
				Enable();
			else
				Disable();
		}

		protected virtual void OnDestroy()
		{
			KillFadeTween();
		}

		protected void KillFadeTween()
		{
			DOTween.Kill(FadeId);
		}

		#endregion Protected Methods
	}
}