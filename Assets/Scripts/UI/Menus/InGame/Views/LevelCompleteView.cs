using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.InGame
{
	public class LevelCompleteView : ViewBase
	{
		#region Private Fields

		[SerializeField]
		private TextMeshProUGUI levelCompleteTmp;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField]
		public Button NextLevelButton { get; private set; }

		[field: SerializeField]
		public Button RestartLevelButton { get; private set; }

		[field: SerializeField]
		public Button ExitToMenuButton { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(string levelCompleteText, bool isLevelComplete, bool isLastLevel, bool fadeUI = true)
		{
			levelCompleteTmp.text = levelCompleteText;
			NextLevelButton.gameObject.SetActive(!isLastLevel);
			SetVisibility(isLevelComplete, fadeUI);
		}

		#endregion Public Methods
	}
}