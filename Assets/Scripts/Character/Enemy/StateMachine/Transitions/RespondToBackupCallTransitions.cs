using UnityEngine;

namespace StealthTD.Enemy.States.Transitions
{
	public class RespondToBackupCallTransitions : EnemyStateTransitions<EnemyStateRespondToBackupCall>
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
				stateHit.SetStateParameters(hitPosition, hitNormal, previousState: stateRespondToBackupCall);
				return stateHit;
			}

			if ((stateRespondToBackupCall.DidReachRequester || localData.IsPlayerDetected)
				&& !sharedData.IsPlayerDead)
			{
				stateChase.SetStateParameters(requestBackup: false);
				return stateChase;
			}

			if (stateRespondToBackupCall.DidStateTimeOut)
				return GetIdleOrPatrolState();

			return null;
		}

		#endregion Protected Methods
	}
}