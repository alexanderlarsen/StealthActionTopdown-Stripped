using System;
using UnityEngine;

namespace StealthTD.UI.MainMenu
{
	[Serializable]
	public class LevelDataModel
	{
		#region Public Properties

		[field: SerializeField]
		public string LevelName { get; private set; }

		[field: SerializeField]
		public int LevelIndex { get; private set; }

		[field: SerializeField]
		public Sprite ThumbnailSprite { get; private set; }

		#endregion Public Properties
	}
}