using StealthTD.Interfaces;
using UnityEngine;

namespace StealthTD.Enemy.AI
{
	public class EnemyHearing : MonoBehaviour, ISoundListener
	{
		#region Private Fields

		[SerializeField, ReadOnly]
		private bool shouldInvestigateSound = false;

		private SoundVolume previousVolume = SoundVolume.None;
		private Vector3 previousPosition = Vector3.negativeInfinity;

		#endregion Private Fields

		#region Public Methods

		public void InvokeHearSound(Vector3 soundPosition, SoundVolume volume, Transform soundOwner)
		{
			if (shouldInvestigateSound)
				return;

			shouldInvestigateSound = GetInvestigateSoundDecision(soundPosition, volume, soundOwner);

			if (!shouldInvestigateSound)
				return;

			shouldInvestigateSound = true;
			previousVolume = volume;
			previousPosition = soundPosition;
		}

		public bool HeardSound(out Vector3 soundPosition, out bool raiseAlert, bool applyRedundancyRange)
		{
			soundPosition = previousPosition;
			raiseAlert = previousVolume > SoundVolume.Medium;

			if (shouldInvestigateSound)
			{
				shouldInvestigateSound = false;
				return true;
			}

			return false;
		}

		public void ForgetPreviousSound()
		{
			previousVolume = SoundVolume.None;
			previousPosition = Vector3.negativeInfinity;
		}

		#endregion Public Methods

		#region Private Methods

		private bool GetInvestigateSoundDecision(Vector3 soundPosition, SoundVolume volume, Transform soundOwner)
		{
			float distance = Vector3.Distance(previousPosition, soundPosition);
			bool isOwnSound = soundOwner == transform;
			bool isEqualToOrLouderThanPrevious = volume >= previousVolume;
			bool isFurtherAway = distance > 1;
			bool isVeryLoud = volume > SoundVolume.Medium;
			bool noPrevious = previousVolume == SoundVolume.None;
			bool isSoftButClose = volume == SoundVolume.Soft && distance <= 1;

			return !isOwnSound && (isEqualToOrLouderThanPrevious && isFurtherAway || isVeryLoud || noPrevious && isSoftButClose);
		}

		#endregion Private Methods
	}
}