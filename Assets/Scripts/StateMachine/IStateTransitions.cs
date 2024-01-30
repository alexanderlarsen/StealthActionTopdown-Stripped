namespace StealthTD.FSM
{
	public interface IStateTransitions
	{
		#region Public Methods

		public bool ShouldTransition(out State nextState);

		#endregion Public Methods
	}
}