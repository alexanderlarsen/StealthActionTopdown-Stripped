using DG.Tweening;
using StealthTD.Enemy.Data;
using StealthTD.UI;
using StealthTD.UI.InGame;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace StealthTD.Enemy.UI
{
	public class EnemyUIController : MonoBehaviour
	{
		#region Private Fields

		[Inject]
		private readonly EnemyLocalData data;

		[SerializeField]
		private CanvasGroup uiRootCanvasGroup;

		[SerializeField]
		private HealthBarView healthBarView;

		[SerializeField]
		private RadialIndicatorView radialTimerView;

		[SerializeField]
		private Sprite unconsciousIcon;

		[SerializeField]
		private Sprite questionMarkIcon;

		[SerializeField]
		private Sprite eyeIcon;

		[SerializeField]
		private Sprite walkieTalkieIcon;

		[SerializeField]
		private Gradient unconsciousGradient;

		[SerializeField]
		private Gradient investigateGradient;

		[SerializeField]
		private Gradient playerVisibleGradient;

		[SerializeField]
		private Gradient requestBackupGradient;

		private bool destructionStarted;

		#endregion Private Fields

		#region Private Properties

		private string UiCanvasRootTweenId => $"{gameObject.GetInstanceID()}_EnemyUI_UiCanvasRootTween";

		#endregion Private Properties

		#region Private Methods

		private void Awake()
		{
			healthBarView.Disable();
			radialTimerView.Disable();
		}

		private void OnEnable()
		{
			data.PropertyChanged += LocalData_PropertyChanged;
		}

		private void OnDisable()
		{
			data.PropertyChanged -= LocalData_PropertyChanged;
		}

		private void OnDestroy()
		{
			DOTween.Kill(UiCanvasRootTweenId);
		}

		private void LocalData_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(data.Health):
				OnHealthChanged();
				break;

				case nameof(data.UnconsciousTimeLeft):
				OnUnconsciousTimeLeftChanged();
				break;

				case nameof(data.IsInvestigating):
				OnIsInvestigatingChanged();
				break;

				case nameof(data.PlayerDetectedAmount):
				OnPlayerDetectedAmountChanged();
				break;

				case nameof(data.RequestBackupTimer):
				OnRequestBackupTimerChanged();
				break;
			}
		}

		private void OnHealthChanged()
		{
			healthBarView.UpdateView(data.Health);

			if (data.Health > 0 || destructionStarted)
				return;

			destructionStarted = true;
			uiRootCanvasGroup.DOFade(0, 0.25f).SetId(UiCanvasRootTweenId);
		}

		private void OnUnconsciousTimeLeftChanged()
		{
			radialTimerView
			.UpdateIcon(unconsciousIcon)
			.UpdateGradient(unconsciousGradient)
			.UpdateFillAmount(data.UnconsciousPercentage);
		}

		private void OnIsInvestigatingChanged()
		{
			if (data.RequestBackupPercentage > 0)
				return;

			radialTimerView
			.UpdateIcon(questionMarkIcon)
			.UpdateGradient(investigateGradient)
			.UpdateFillAmount(data.IsInvestigating ? 1 : 0);
		}

		private void OnPlayerDetectedAmountChanged()
		{
			if (data.UnconsciousPercentage > 0)
				return;

			if (data.RequestBackupTimer > 0)
				return;

			if (data.IsInvestigating && data.PlayerDetectedPercentage <= 0)
			{
				radialTimerView
				.UpdateIcon(questionMarkIcon)
				.UpdateGradient(investigateGradient)
				.UpdateFillAmount(1);
				return;
			}

			radialTimerView
			.UpdateIcon(eyeIcon)
			.UpdateGradient(playerVisibleGradient)
			.UpdateFillAmount(data.PlayerDetectedPercentage);
		}

		private void OnRequestBackupTimerChanged()
		{
			if (data.UnconsciousPercentage > 0)
				return;

			radialTimerView
			.UpdateIcon(walkieTalkieIcon)
			.UpdateGradient(requestBackupGradient)
			.UpdateFillAmount(data.RequestBackupPercentage);

			if (data.RequestBackupPercentage == 0)
			{
				radialTimerView
				.UpdateIcon(eyeIcon)
				.UpdateGradient(playerVisibleGradient)
				.UpdateFillAmount(data.PlayerDetectedPercentage);
			}
		}

		#endregion Private Methods
	}
}