using StealthTD.GameLogic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Zenject;

namespace StealthTD.UI.MainMenu
{
	public class MenuSceneModel : INotifyPropertyChanged
	{
		#region Private Fields

		[Inject]
		private readonly LevelManager levelManager;

		private MenuView currentView;

		#endregion Private Fields

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Public Events

		#region Public Properties

		public MenuView CurrentView
		{
			get => currentView;
			set
			{
				currentView = value;
				InvokePropertyChanged();
			}
		}

		public int CompletedLevelsCount => levelManager.CompletedLevelsCount;

		#endregion Public Properties

		#region Public Methods

		public bool IsLevelLocked(int levelIndex)
		{
			return levelIndex > CompletedLevelsCount + 1;
		}

		public void LoadLevel(int levelIndex)
		{
			levelManager.LoadLevel(levelIndex);
		}

		#endregion Public Methods

		#region Private Methods

		private void InvokePropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion Private Methods
	}
}