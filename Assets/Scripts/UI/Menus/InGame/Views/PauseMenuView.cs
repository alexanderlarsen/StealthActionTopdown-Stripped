using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.InGame
{
	public class PauseMenuView : ViewBase
	{
		#region Public Properties

		[field: SerializeField]
		public Button ResumeButton { get; private set; }

		[field: SerializeField]
		public Button RestartLevelButton { get; private set; }

		[field: SerializeField]
		public Button ExitToMenuButton { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(bool isPaused, bool fadeUI = true)
		{
			SetVisibility(isPaused, fadeUI);
		}

		#endregion Public Methods
	}
}