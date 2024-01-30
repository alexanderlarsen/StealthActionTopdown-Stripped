using StealthTD.Enemy.AI;
using StealthTD.Enemy.States;
using StealthTD.Enemy.States.Transitions;
using StealthTD.FSM;
using StealthTD.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace StealthTD.Enemy
{
	public class EnemyStateTransitionManager : StateTransitionManager<EnemyState>
	{
		#region Private Fields

		[Inject]
		private readonly ChaseStateTransitions chaseStateTransitions;

		[Inject]
		private readonly HitStateTransitions hitStateTransitions;

		[Inject]
		private readonly IdleStateTransitions idleStateTransitions;

		[Inject]
		private readonly InvestigationStateTransitions investigationStateTransitions;

		[Inject]
		private readonly PatrolStateTransitions patrolStateTransitions;

		[Inject]
		private readonly RespondToBackupCallTransitions respondToBackupCallTransitions;

		[Inject]
		private readonly UnconsciousStateTransitions unconsciousStateTransitions;

		[Inject]
		private readonly EnemyStateIdle stateIdle;

		[Inject]
		private readonly EnemyStatePatrol statePatrol;

		[Inject]
		private readonly EnemyPatrolPath patrolPath;

		[Inject]
		private readonly ISoundListener soundListener;

		#endregion Private Fields

		#region Protected Methods

		protected override void OnStateChange(State nextState)
		{
			if (nextState is not EnemyStateInvestigate)
				soundListener.ForgetPreviousSound();
		}

		protected override EnemyState GetInitialState()
		{
			if (patrolPath != null && patrolPath.NumPoints > 1)
				return statePatrol;

			return stateIdle;
		}

		protected override List<IStateTransitions> DefineStateTransitions()
		{
			return new()
			{
				chaseStateTransitions,
				hitStateTransitions,
				idleStateTransitions,
				investigationStateTransitions,
				patrolStateTransitions,
				respondToBackupCallTransitions,
				unconsciousStateTransitions
			};
		}

		#endregion Protected Methods
	}
}