using StealthTD.FSM;
using StealthTD.Player.Data;
using StealthTD.Player.Movement;
using Zenject;

namespace StealthTD.Player.States.Transitions
{
	public abstract class PlayerStateTransitions<TargetState> : StateTransitions<TargetState, PlayerState>
		where TargetState : PlayerState
	{
		#region Protected Fields

		[Inject]
		protected readonly PlayerStateDeath stateDeath;

		[Inject]
		protected readonly PlayerStateFalling stateFalling;

		[Inject]
		protected readonly PlayerStateMove stateMove;

		[Inject]
		protected readonly PlayerLocalData localData;

		[Inject]
		protected readonly PlayerMoveController moveController;

		#endregion Protected Fields
	}
}