namespace StealthTD.Player.States.Transitions
{
	public class StateMoveTransitions : PlayerStateTransitions<PlayerStateMove>
	{
		#region Protected Methods

		protected override PlayerState GetNextState()
		{
			if (localData.Health <= 0)
				return stateDeath;

			if (moveController.TimeNotGrounded >= 0.05f)
				return stateFalling;

			return null;
		}

		#endregion Protected Methods
	}
}