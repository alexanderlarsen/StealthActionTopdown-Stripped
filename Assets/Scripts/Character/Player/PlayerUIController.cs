using DG.Tweening;
using StealthTD.UI;
using StealthTD.UI.Player;
using UnityEngine;
using Zenject;

namespace StealthTD.Player.UI
{
	public class PlayerUIController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly PlayerUIModel model;

		[SerializeField]
		private AmmoView ammoView;

		[SerializeField]
		private EquipView equipView;

		[SerializeField]
		private ReloadView reloadView;

		[SerializeField]
		private HealthBarView healthBar;

		[SerializeField]
		private CanvasGroup uiRootCanvasGroup;

		private bool destructionStarted;

		#endregion Private Fields

		#region Private Properties

		private string UiRootCanvasGroupTweenId => $"{gameObject.GetInstanceID()}_Player_UiRootCanvasGroupTween";

		#endregion Private Properties

		#region Private Methods

		private void Awake()
		{
			model.PropertyChanged += Model_PropertyChanged;
		}

		private void OnDestroy()
		{
			model.PropertyChanged -= Model_PropertyChanged;
			DOTween.Kill(UiRootCanvasGroupTweenId);
		}

		private void Start()
		{
			equipView.Disable();
			ammoView.Disable();
			reloadView.Disable();
		}

		private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(model.HasShootingWeapon):
				ammoView.UpdateView(model.CurrentAmmo, model.MaxAmmo, model.HasShootingWeapon, model.ShouldReload);
				break;

				case nameof(model.ShouldReload):
				ammoView.UpdateView(model.CurrentAmmo, model.MaxAmmo, model.HasShootingWeapon, model.ShouldReload);
				reloadView.UpdateView(model.ShouldReload);
				break;

				case nameof(model.CurrentAmmo):
				ammoView.UpdateView(model.CurrentAmmo, model.MaxAmmo, model.HasShootingWeapon, model.ShouldReload);
				break;

				case nameof(model.MaxAmmo):
				ammoView.UpdateView(model.CurrentAmmo, model.MaxAmmo, model.HasShootingWeapon, model.ShouldReload);
				break;

				case nameof(model.CurrentEquipable):
				equipView.UpdateView(model.CurrentEquipable);
				break;

				case nameof(model.CurrentHealth):
				healthBar.UpdateView(model.CurrentHealth);
				DisposeIfPlayerDead();
				break;
			}
		}

		private void DisposeIfPlayerDead()
		{
			if (model.CurrentHealth > 0 || destructionStarted)
				return;

			destructionStarted = true;
			uiRootCanvasGroup.DOFade(0, 0.4f).OnComplete(() => Destroy(this)).SetId(UiRootCanvasGroupTweenId);
		}

		#endregion Private Methods
	}
}