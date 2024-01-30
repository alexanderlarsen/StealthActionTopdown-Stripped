using StealthTD.Audio;
using StealthTD.GameLogic;
using StealthTD.UI;
using StealthTD.UI.MainMenu;
using Zenject;

namespace StealthTD.Installers
{
	public class MainMenuInstaller : MonoInstaller
	{
		#region Public Methods

		public override void InstallBindings()
		{
			Container.Bind<CursorController>().FromComponentInHierarchy().AsSingle();
			Container.Bind<DOTweenKillAllHelper>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
			Container.Bind<LevelManager>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
			Container.Bind<LevelSelectorView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<MenuAudioManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<MenuSceneController>().FromComponentInHierarchy().AsSingle();
			Container.Bind<TitleScreenView>().FromComponentInHierarchy().AsSingle();

			Container.Bind<MenuSceneModel>().AsSingle();
		}

		#endregion Public Methods
	}
}