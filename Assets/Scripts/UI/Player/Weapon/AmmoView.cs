using DG.Tweening;
using TMPro;
using UnityEngine;

namespace StealthTD.UI.Player
{
	public class AmmoView : ViewBase
	{
		#region Private Fields

		private const string id = "player_ammoTmpColor";

		[SerializeField]
		private TextMeshProUGUI ammoTmp;

		private bool isVisible;

		#endregion Private Fields

		#region Public Methods

		public void UpdateView(int currentAmmo, int maxAmmo, bool hasShootingWeapon, bool shouldReload)
		{
			if (hasShootingWeapon && !isVisible)
			{
				isVisible = true;
				FadeIn(0.2f);
			}
			else if (!hasShootingWeapon && isVisible)
			{
				isVisible = false;
				FadeOut(0.2f);
			}

			if (!hasShootingWeapon)
				return;

			UpdateText(currentAmmo, maxAmmo);
			UpdateTextColor(currentAmmo, shouldReload);
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void OnDestroy()
		{
			base.OnDestroy();
			DOTween.Kill(id);
		}

		#endregion Protected Methods

		#region Private Methods

		private void UpdateText(int currentAmmo, int maxAmmo)
		{
			ammoTmp.text = $"Ammo: {currentAmmo}/{maxAmmo}";
		}

		private void UpdateTextColor(int currentAmmo, bool shouldReload)
		{
			Color targetColor;

			if (currentAmmo == 0)
				targetColor = Color.red;
			else if (shouldReload)
				targetColor = Color.yellow;
			else
				targetColor = Color.white;

			DOTween.Kill(id);
			ammoTmp.DOColor(targetColor, 0.2f).SetId(id);
		}

		#endregion Private Methods
	}
}