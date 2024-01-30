using StealthTD.FSM;
using StealthTD.Player.States;
using StealthTD.Player.States.Transitions;
using System.Collections.Generic;
using Zenject;

namespace StealthTD.Player
{
	public class PlayerStateTransitionManager : StateTransitionManager<PlayerState>
	{
		#region Private Fields

		[Inject]
		private readonly StateMoveTransitions moveTransitions;

		[Inject]
		private readonly StateFallingTransitions fallingTransitions;

		[Inject]
		private readonly PlayerStateMove stateMove;

		#endregion Private Fields

		#region Protected Methods

		protected override List<IStateTransitions> DefineStateTransitions()
		{
			return new()
			{
				moveTransitions,
				fallingTransitions
			};
		}

		protected override PlayerState GetInitialState()
		{
			return stateMove;
		}

		#endregion Protected Methods
	}
}