using StealthTD.Animation;
using StealthTD.Character;
using StealthTD.FSM;
using StealthTD.Player;
using StealthTD.Player.Data;
using StealthTD.Player.Movement;
using StealthTD.Player.States;
using StealthTD.Player.States.Transitions;
using StealthTD.Player.Weapons;
using StealthTD.UI.Player;
using UnityEngine;
using Zenject;

namespace StealthTD.Installers
{
	public class PlayerInstaller : MonoInstaller
	{
		#region Public Methods

		public override void InstallBindings()
		{
			Container.Bind<AnimationEventRelayer>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<Animator>().FromComponentsOn(gameObject).AsSingle();
			Container.Bind<CharacterBoneReferences>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<CharacterController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerEquipManager>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerLocalData>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerMoveController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerPunchController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerStateDeath>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerStateFalling>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerStateMove>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<PlayerWeaponController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<StateMachine>().FromComponentOn(gameObject).AsSingle();

			Container.Bind<StateMoveTransitions>().AsSingle();
			Container.Bind<StateFallingTransitions>().AsSingle();
			Container.BindInterfacesAndSelfTo<PlayerUIModel>().AsSingle();
		}

		#endregion Public Methods

		#region Private Methods

		private void OnDestroy()
		{
			Container.UnbindAll();
		}

		#endregion Private Methods
	}
}