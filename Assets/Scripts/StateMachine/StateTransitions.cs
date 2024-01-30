using Zenject;

namespace StealthTD.FSM
{
	public abstract class StateTransitions<TargetState, BaseState> : IStateTransitions
		where TargetState : State
		where BaseState : State
	{
		#region Private Fields

		[Inject]
		private readonly StateMachine stateMachine;

		#endregion Private Fields

		#region Public Methods

		public bool ShouldTransition(out State nextState)
		{
			nextState = stateMachine.CurrentState is TargetState ? GetNextState() : null;
			return nextState != null;
		}

		#endregion Public Methods

		#region Protected Methods

		protected abstract BaseState GetNextState();

		#endregion Protected Methods
	}
}