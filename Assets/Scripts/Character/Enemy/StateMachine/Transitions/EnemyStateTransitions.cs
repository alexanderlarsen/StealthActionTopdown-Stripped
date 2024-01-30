using StealthTD.Enemy.AI;
using StealthTD.Enemy.Data;
using StealthTD.FSM;
using Zenject;

namespace StealthTD.Enemy.States.Transitions
{
	public abstract class EnemyStateTransitions<TargetState> : StateTransitions<TargetState, EnemyState>
		where TargetState : EnemyState
	{
		#region Protected Fields

		[Inject]
		protected readonly EnemyStateChase stateChase;

		[Inject]
		protected readonly EnemyStateDeath stateDeath;

		[Inject]
		protected readonly EnemyStateHit stateHit;

		[Inject]
		protected readonly EnemyStateIdle stateIdle;

		[Inject]
		protected readonly EnemyStateInvestigate stateInvestigate;

		[Inject]
		protected readonly EnemyStatePatrol statePatrol;

		[Inject]
		protected readonly EnemyStateRespondToBackupCall stateRespondToBackupCall;

		[Inject]
		protected readonly EnemyStateUnconscious stateUnconscious;

		[Inject]
		protected readonly EnemyLocalData localData;

		[Inject]
		protected readonly EnemySharedData sharedData;

		[Inject]
		protected readonly EnemyVision vision;

		[Inject]
		protected readonly EnemyHearing hearing;

		[Inject]
		protected readonly EnemyPatrolPath patrolPath;

		[Inject]
		protected readonly EnemyAgent enemy;

		[Inject]
		protected EnemyMoveController moveController;

		#endregion Protected Fields

		#region Protected Methods

		protected EnemyState GetIdleOrPatrolState()
		{
			if (patrolPath != null && patrolPath.NumPoints > 1)
				return statePatrol;

			return stateIdle;
		}

		#endregion Protected Methods
	}
}