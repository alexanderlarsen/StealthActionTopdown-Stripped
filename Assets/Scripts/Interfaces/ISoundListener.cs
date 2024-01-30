using UnityEngine;

namespace StealthTD.Interfaces
{
	public interface ISoundListener
	{
		void InvokeHearSound(Vector3 soundPosition, SoundVolume volume, Transform soundOwner);

		bool HeardSound(out Vector3 soundPosition, out bool raiseAlert, bool ignoreSoundIfClose);

		void ForgetPreviousSound();
	}
}