using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.InGame
{
	public class GameOverMenuView : ViewBase
	{
		#region Public Properties

		[field: SerializeField]
		public Button RestartLevelButton { get; private set; }

		[field: SerializeField]
		public Button ExitToMenuButton { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(bool isGameOver, bool fadeUI = true)
		{
			SetVisibility(isGameOver, fadeUI);
		}

		#endregion Public Methods
	}
}