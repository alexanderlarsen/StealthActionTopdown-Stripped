using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.MainMenu
{
	public class LevelSelectorView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private GameObject levelButtonPrefab;

		[SerializeField]
		private GridLayoutGroup gridLayoutGroup;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField]
		public Button BackButton { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(MenuView currentView)
		{
			if (currentView == MenuView.LevelSelector)
				FadeIn(0.5f);
			else
				Disable();
		}

		public LevelSelectorButtonView CreateButton()
		{
			LevelSelectorButtonView view =
					Instantiate(levelButtonPrefab, gridLayoutGroup.transform)
					.GetComponent<LevelSelectorButtonView>();

			return view;
		}

		#endregion Public Methods
	}
}