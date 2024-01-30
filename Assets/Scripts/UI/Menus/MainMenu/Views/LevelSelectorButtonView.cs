using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StealthTD.UI.MainMenu
{
	public class LevelSelectorButtonView : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		private TextMeshProUGUI titleTmp;

		[SerializeField]
		private Image thumbnail;

		[SerializeField]
		private Image lockIcon;

		#endregion Private Fields

		#region Public Properties

		[field: SerializeField]
		public Button Button { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(string levelName, bool isLocked, Sprite thumbnailSprite)
		{
			lockIcon.gameObject.SetActive(isLocked);
			titleTmp.text = levelName;

			if (thumbnailSprite != null)
				thumbnail.sprite = thumbnailSprite;
		}

		#endregion Public Methods
	}
}