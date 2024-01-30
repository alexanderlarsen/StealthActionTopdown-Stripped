using StealthTD.Extensions;
using System;
using UnityEngine;

namespace StealthTD.Audio
{
	[Serializable]
	public class ImpactAudioHandler
	{
		#region Private Fields

		[SerializeField]
		private AudioClip hitConcrete;

		[SerializeField]
		private AudioClip hitBody;

		[SerializeField]
		private AudioClip explosion;

		#endregion Private Fields

		#region Public Methods

		public void PlayImpactAudio(SurfaceType surfaceMaterial, Vector3 position, Transform soundOwner)
		{
			switch (surfaceMaterial)
			{
				case SurfaceType.Concrete:
				hitConcrete.PlayAndNotifyListeners(position, SoundVolume.Medium, soundOwner);
				break;

				case SurfaceType.Body:
				hitBody.PlayAndNotifyListeners(position, SoundVolume.Soft, soundOwner);
				break;

				default:
				hitConcrete.PlayAndNotifyListeners(position, SoundVolume.Medium, soundOwner);
				break;
			};
		}

		public void PlayExplosionAudio(Vector3 position, Transform soundOwner)
		{
			explosion.PlayAndNotifyListeners(position, SoundVolume.VeryLoud, soundOwner);
		}

		#endregion Public Methods
	}
}