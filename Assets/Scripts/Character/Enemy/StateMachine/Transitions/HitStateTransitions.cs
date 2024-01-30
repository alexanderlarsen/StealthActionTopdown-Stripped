using UnityEngine;

namespace StealthTD.Enemy.States.Transitions
{
	public class HitStateTransitions : EnemyStateTransitions<EnemyStateHit>
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
				stateHit.SetStateParameters(hitPosition, hitNormal, previousState: stateHit.PreviousState);
				return stateHit;
			}

			if(stateHit.PreviousState is not EnemyStateChase and not EnemyStateRespondToBackupCall && stateHit.IsPlayerDetected)
			{
				stateChase.SetStateParameters(requestBackup: true);
				return stateChase;
			}

			if (stateHit.DidStateTimeOut)
			{
				if (stateHit.PreviousState is EnemyStateChase)
					stateChase.SetStateParameters(requestBackup: stateChase.WasBackupRequestInterfered);

				return stateHit.PreviousState;
			}

			return null;
		}

		#endregion Protected Methods
	}
}