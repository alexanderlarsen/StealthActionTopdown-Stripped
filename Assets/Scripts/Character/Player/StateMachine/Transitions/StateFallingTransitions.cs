namespace StealthTD.Player.States.Transitions
{
	public class StateFallingTransitions : PlayerStateTransitions<PlayerStateFalling>
	{
		protected override PlayerState GetNextState()
		{
			if (localData.Health <= 0)
				return stateDeath;

			if (stateFalling.IsFallComplete)
				return stateMove;

			return null;
		}
	}
}