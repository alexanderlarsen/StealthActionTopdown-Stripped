using StealthTD.Audio;
using StealthTD.CameraControllers;
using StealthTD.Enemy;
using StealthTD.GameLogic;
using StealthTD.Input;
using StealthTD.ObjectPool;
using StealthTD.Player;
using StealthTD.UI;
using StealthTD.UI.InGame;
using StealthTD.VFX;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace StealthTD.Installers
{
	public class GameInstaller : MonoInstaller
	{
		#region Public Methods

		public override void InstallBindings()
		{
			Container.Bind<AudioManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
			Container.Bind<CursorController>().FromComponentInHierarchy().AsSingle();
			Container.Bind<DOTweenKillAllHelper>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
			Container.Bind<EnemySharedData>().FromComponentInHierarchy().AsSingle();
			Container.Bind<GameOverMenuView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<GameStateManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<GoalMarkerController>().FromComponentInHierarchy().AsSingle();
			Container.Bind<LevelCompleteView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<ObjectivesView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<LevelManager>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
			Container.Bind<MenuAudioManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<PauseMenuView>().FromComponentInHierarchy().AsSingle();
			Container.Bind<PlayerAgent>().FromComponentInHierarchy().AsSingle();
			Container.Bind<PlayerInput>().FromComponentInHierarchy().AsSingle();
			Container.Bind<PrefabManager>().FromComponentInHierarchy().AsSingle();
			Container.Bind<Transform>().WithId("PlayerTransform").FromMethod(ctx => ctx.Container.Resolve<PlayerAgent>().transform).AsSingle();
			Container.Bind<TrackPlayerCameraController>().FromComponentInHierarchy().AsSingle();
			Container.Bind<ObjectiveManager>().FromComponentInHierarchy().AsSingle();

			Container.Bind<InputProvider>().AsSingle();
			Container.Bind<VfxManager>().AsSingle();
			Container.BindInterfacesAndSelfTo<InGameUIModel>().AsSingle();

			Container.Bind<EnemyAgent>().FromComponentsInHierarchy().AsSingle();
			Container.Bind<Keycard>().FromComponentsInHierarchy().AsSingle();
		}

		#endregion Public Methods
	}
}