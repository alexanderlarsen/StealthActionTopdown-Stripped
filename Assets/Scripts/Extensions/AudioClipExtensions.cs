using StealthTD.Interfaces;
using UnityEngine;

namespace StealthTD.Extensions
{
	public static class AudioClipExtensions
	{
		#region Public Methods

		public static void PlayAndNotifyListeners(this AudioClip audioClip, Vector3 position, SoundVolume volume, Transform soundOwner)
		{
			AudioSource.PlayClipAtPoint(audioClip, position);
			NotifyListeners(position, volume, soundOwner);
		}

		public static void PlayAndNotifyListeners(this AudioClip[] audioClips, Vector3 position, SoundVolume volume, Transform soundOwner)
		{
			foreach (AudioClip audioClip in audioClips)
				AudioSource.PlayClipAtPoint(audioClip, position);

			NotifyListeners(position, volume, soundOwner);
		}

		#endregion Public Methods

		#region Private Methods

		private static void NotifyListeners(Vector3 position, SoundVolume volume, Transform soundOwner)
		{
			if (volume == SoundVolume.Soft)
				return;

			float range = volume switch
			{
				SoundVolume.Soft => 0,
				SoundVolume.Medium => 7,
				SoundVolume.Loud => 25,
				SoundVolume.VeryLoud => 50,
				_ => throw new System.NotImplementedException()
			};

			Collider[] listenersInRange = Physics.OverlapSphere(position, range, LayerMask.GetMask("Enemy"));

			foreach (Collider col in listenersInRange)
				if (col.TryGetComponent(out ISoundListener soundListener))
					soundListener.InvokeHearSound(position, volume, soundOwner);
		}

		#endregion Private Methods
	}
}