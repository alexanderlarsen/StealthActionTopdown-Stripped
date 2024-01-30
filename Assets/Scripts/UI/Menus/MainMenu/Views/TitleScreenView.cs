using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace StealthTD.UI.MainMenu
{
	public class TitleScreenView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private TextMeshProUGUI pressAnyKeyTmp;

		private Coroutine animatePressAnyKeyRoutine;

		#endregion Private Fields

		#region Public Methods

		public void UpdateView(MenuView currentView)
		{
			if (currentView == MenuView.TitleScreen)
				FadeIn(2);
			else
				FadeOut(0.2f);
		}

		#endregion Public Methods

		#region Private Methods

		private void OnEnable()
		{
			animatePressAnyKeyRoutine = StartCoroutine(AnimatePressAnyKeyTextRoutine());
		}

		private void OnDisable()
		{
			if (animatePressAnyKeyRoutine != null)
				StopCoroutine(animatePressAnyKeyRoutine);
		}

		private IEnumerator AnimatePressAnyKeyTextRoutine()
		{
			while (true)
			{
				yield return pressAnyKeyTmp.DOFade(0.1f, 1f).WaitForCompletion();
				yield return pressAnyKeyTmp.DOFade(1, 1f).WaitForCompletion();
			}
		}

		#endregion Private Methods
	}
}