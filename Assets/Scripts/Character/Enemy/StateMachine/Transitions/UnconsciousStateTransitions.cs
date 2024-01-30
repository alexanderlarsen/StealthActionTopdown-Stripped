namespace StealthTD.Enemy.States.Transitions
{
	public class UnconsciousStateTransitions : EnemyStateTransitions<EnemyStateUnconscious>
	{
		#region Protected Methods

		protected override EnemyState GetNextState()
		{
			if (localData.IsDead)
				return stateDeath;

			if (!localData.IsUnconscious)
				return GetIdleOrPatrolState();

			return null;
		}

		#endregion Protected Methods
	}
}