using StealthTD.Animation;
using StealthTD.Character;
using StealthTD.Enemy;
using StealthTD.Enemy.AI;
using StealthTD.Enemy.Data;
using StealthTD.Enemy.States;
using StealthTD.Enemy.States.Transitions;
using StealthTD.FSM;
using StealthTD.Helpers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace StealthTD.Installers
{
	public class EnemyInstaller : MonoInstaller
	{
		#region Private Fields

		[SerializeField]
		private EnemyPatrolPath patrolPath;

		#endregion Private Fields

		#region Public Methods

		public override void InstallBindings()
		{
			Container.Bind<AnimationEventRelayer>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<Animator>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<CharacterBoneReferences>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<Disposer>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyMoveController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyPatrolPath>().FromInstance(patrolPath).AsSingle();
			Container.Bind<EnemyStateChase>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateDeath>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateHit>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateIdle>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateInvestigate>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStatePatrol>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateRespondToBackupCall>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyStateUnconscious>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyVision>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<EnemyWeaponController>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<NavMeshAgent>().FromComponentOn(gameObject).AsSingle();
			Container.Bind<StateMachine>().FromComponentOn(gameObject).AsSingle();
			Container.BindInterfacesAndSelfTo<EnemyAgent>().FromComponentOn(gameObject).AsSingle();
			Container.BindInterfacesAndSelfTo<EnemyHearing>().FromComponentOn(gameObject).AsSingle();
			Container.BindInterfacesAndSelfTo<EnemyLocalData>().FromComponentOn(gameObject).AsSingle();

			Container.Bind<ChaseStateTransitions>().AsSingle();
			Container.Bind<HitStateTransitions>().AsSingle();
			Container.Bind<IdleStateTransitions>().AsSingle();
			Container.Bind<InvestigationStateTransitions>().AsSingle();
			Container.Bind<PatrolStateTransitions>().AsSingle();
			Container.Bind<RespondToBackupCallTransitions>().AsSingle();
			Container.Bind<UnconsciousStateTransitions>().AsSingle();
		}

		#endregion Public Methods
	}
}