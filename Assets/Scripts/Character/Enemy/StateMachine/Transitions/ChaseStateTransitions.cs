using UnityEngine;

namespace StealthTD.Enemy.States.Transitions
{
	public class ChaseStateTransitions : EnemyStateTransitions<EnemyStateChase>
	{
		#region Protected Methods

		protected override EnemyState GetNextState()
		{
			if (localData.IsDead)
				return stateDeath;

			if (localData.IsUnconscious)
				return stateUnconscious;

			if (localData.WasHit(out Vector3 hitPosition, out Vector3 hitNormal))
			{
				stateHit.SetStateParameters(hitPosition, hitNormal, stateChase);
				return stateHit;
			}

			if (stateChase.HasLostTarget || sharedData.IsPlayerDead)
				return GetIdleOrPatrolState();

			return null;
		}

		#endregion Protected Methods
	}
}