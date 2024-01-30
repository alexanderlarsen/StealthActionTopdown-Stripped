using StealthTD.Extensions;
using System;
using UnityEngine;

namespace StealthTD.Audio
{
	[Serializable]
	public class CharacterAudioHandler
	{
		#region Private Fields

		[SerializeField]
		private AudioClip[] overheadKnockoutAudio;

		[SerializeField]
		private AudioClip bodyDropAudio;

		[SerializeField]
		private AudioClip hitAudio;

		[SerializeField]
		private AudioClip deathAudio;

		[SerializeField]
		private AudioClip suspiciousAudio;

		[SerializeField]
		private AudioClip alertedAudio;

		[SerializeField]
		private AudioClip punchAudio;

		#endregion Private Fields

		#region Public Methods

		public void PlayPunchAudio(Vector3 position, Transform soundOwner) =>
			punchAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlayBodyDropAudio(Vector3 position, Transform soundOwner) => bodyDropAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlayOverheadKnockOutAudio(Vector3 position, Transform soundOwner) => overheadKnockoutAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlayHitAudio(Vector3 position, Transform soundOwner) => hitAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlayDeathAudio(Vector3 position, Transform soundOwner) => deathAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlaySuspiciousAudio(Vector3 position, Transform soundOwner) => suspiciousAudio.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);

		public void PlayAlertedAudio(Vector3 position, Transform soundOwner) => alertedAudio.PlayAndNotifyListeners(position, SoundVolume.Medium, soundOwner);

		#endregion Public Methods
	}
}