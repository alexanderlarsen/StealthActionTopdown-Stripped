using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI
{
	public class HealthBarView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private Slider slider;

		#endregion Private Fields

		#region Public Methods

		public void UpdateView(int currentHealth)
		{
			FadeIn(0.2f);
			StopAllCoroutines();
			StartCoroutine(HandleHealthChanged(currentHealth));
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void OnDestroy()
		{
			base.OnDestroy();
			DOTween.Kill(this);
		}

		#endregion Protected Methods

		#region Private Methods

		private void Start()
		{
			Disable();
		}

		private IEnumerator HandleHealthChanged(int currentHealth)
		{
			DOTween.Kill(this);
			slider.DOValue(currentHealth, 0.15f).SetId(this);
			yield return new WaitForSeconds(2f);
			FadeOut(0.2f);
		}

		#endregion Private Methods
	}
}