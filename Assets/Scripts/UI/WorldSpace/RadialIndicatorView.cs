using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.InGame
{
	public class RadialIndicatorView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private Image icon;

		[SerializeField]
		private Image circle;

		private Gradient gradient;
		private bool isVisible;

		#endregion Private Fields

		#region Public Methods

		public RadialIndicatorView UpdateIcon(Sprite iconSprite)
		{
			if (icon.sprite != iconSprite)
				icon.sprite = iconSprite;

			bool active = icon.sprite != null;

			if (icon.gameObject.activeSelf != active)
				icon.gameObject.SetActive(active);

			return this;
		}

		public RadialIndicatorView UpdateGradient(Gradient gradient)
		{
			if (this.gradient != gradient)
				this.gradient = gradient;

			return this;
		}

		public RadialIndicatorView UpdateFillAmount(float fillAmount)
		{
			if (fillAmount > 0 && !isVisible)
			{
				isVisible = true;
				FadeIn(0.2f);
			}
			else if (fillAmount == 0 && isVisible)
			{
				isVisible = false;
				FadeOut(0.2f);
			}

			circle.fillAmount = fillAmount;
			circle.color = gradient.Evaluate(fillAmount);
			icon.color = gradient.Evaluate(fillAmount);
			return this;
		}

		#endregion Public Methods
	}
}