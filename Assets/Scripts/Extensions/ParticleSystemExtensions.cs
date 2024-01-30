using UnityEngine;

namespace StealthTD.Extensions
{
	public static class ParticleSystemExtensions
	{
		#region Public Methods

		public static void SetDuration(this ParticleSystem particleSystem, float duration)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.duration = duration;
		}

		#endregion Public Methods
	}
}