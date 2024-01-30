using UnityEngine;

namespace StealthTD.Enemy.States.Transitions
{
	public class InvestigationStateTransitions : EnemyStateTransitions<EnemyStateInvestigate>
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
				stateHit.SetStateParameters(hitPosition, hitNormal, previousState: stateInvestigate);
				return stateHit;
			}

			if (localData.IsPlayerDetected && !sharedData.IsPlayerDead)
			{
				stateChase.SetStateParameters(requestBackup: true);
				return stateChase;
			}

			if (localData.WasCalledForBackup(out Transform backupRequesterTransform))
			{
				stateRespondToBackupCall.SetStateParameters(backupRequesterTransform);
				return stateRespondToBackupCall;
			}

			if (!stateInvestigate.IsInvestigatingBody
				&& vision.CanSeeIncapacitatedEnemy(out EnemyAgent enemy))
			{
				if (moveController.IsValidPosition(enemy.HeadTransform.position))
				{
					stateInvestigate.SetStateParameters(
								investigationPosition: enemy.HeadTransform.position,
								didTransitionToSelf: true,
								isInvestigatingBody: true,
								isAlerted: enemy.IsDead);

					return stateInvestigate;
				}

				enemy.TryMarkDeadBodyAsDiscovered();
			}

			if (!stateInvestigate.IsInvestigatingBody
				&& hearing.HeardSound(out Vector3 soundPosition, out bool raiseAlert, false))
			{
				stateInvestigate.SetStateParameters(
					investigationPosition: soundPosition,
					didTransitionToSelf: true,
					isInvestigatingBody: false,
					isAlerted: raiseAlert);

				return stateInvestigate;
			}

			if (!localData.IsInvestigating)
				return GetIdleOrPatrolState();

			return null;
		}

		#endregion Protected Methods
	}
}