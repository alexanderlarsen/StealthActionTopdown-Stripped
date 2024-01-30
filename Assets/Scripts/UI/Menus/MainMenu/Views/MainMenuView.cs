using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.MainMenu
{
	public class MainMenuView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private TextMeshProUGUI titleTmp;

		[SerializeField]
		private TextMeshProUGUI appVersionTmp;

		[SerializeField]
		private CanvasGroup buttons;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField]
		public Button StartGameButton { get; private set; }

		[field: SerializeField]
		public Button ContinueGameButton { get; private set; }

		[field: SerializeField]
		public Button SettingsButton { get; private set; }

		[field: SerializeField]
		public Button QuitGameButton { get; private set; }

		

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(MenuView currentView, int numCompletedLevels, string appVersion)
		{
			if (currentView == MenuView.MainMenu)
				FadeIn(0.5f);
			else
				Disable();

			StartGameButton.gameObject.SetActive(numCompletedLevels == 0);
			ContinueGameButton.gameObject.SetActive(numCompletedLevels > 0);
			appVersionTmp.text = $"Version {appVersion}"; 
		}

		#endregion Public Methods

		#region Private Methods

		private IEnumerator Start()
		{
			KillFadeTween();
			canvasGroup.alpha = 1;
			buttons.alpha = 0;
			appVersionTmp.alpha = 0; 
			buttons.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.2f);
			titleTmp.transform.DOLocalMoveY(310, 0.75f);
			yield return new WaitForSeconds(0.375f);
			buttons.gameObject.SetActive(true);
			buttons.DOFade(1, 1f);
			appVersionTmp.DOFade(1, 1f);
		}

		#endregion Private Methods
	}
}