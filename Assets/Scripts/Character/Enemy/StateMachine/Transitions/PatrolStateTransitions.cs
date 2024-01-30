using UnityEngine;

namespace StealthTD.Enemy.States.Transitions
{
	public class PatrolStateTransitions : EnemyStateTransitions<EnemyStatePatrol>
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
				stateHit.SetStateParameters(hitPosition, hitNormal, previousState: statePatrol);
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

			if (vision.CanSeeIncapacitatedEnemy(out EnemyAgent enemy))
			{
				if (moveController.IsValidPosition(enemy.HeadTransform.position))
				{
					stateInvestigate.SetStateParameters(
					investigationPosition: enemy.HeadTransform.position,
					didTransitionToSelf: false,
					isInvestigatingBody: true,
					isAlerted: enemy.IsDead);

					return stateInvestigate;
				}

				enemy.TryMarkDeadBodyAsDiscovered();
			}

			if (hearing.HeardSound(out Vector3 soundPosition, out bool raiseAlert, false))
			{
				stateInvestigate.SetStateParameters(
					investigationPosition: soundPosition,
					didTransitionToSelf: false,
					isInvestigatingBody: false,
					isAlerted: raiseAlert);

				return stateInvestigate;
			}

			return null;
		}

		#endregion Protected Methods
	}
}