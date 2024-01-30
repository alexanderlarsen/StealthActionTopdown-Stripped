using StealthTD.Extensions;
using StealthTD.ObjectPool;
using UnityEngine;

namespace StealthTD.VFX
{
	public class SleepingVfx : PooledObject<SleepingVfx>
	{
		#region Private Fields

		[SerializeField]
		private ParticleSystem particle;

		#endregion Private Fields

		#region Public Methods

		public void SetDuration(float duration)
		{
			particle.Stop();
			particle.SetDuration(duration);
			particle.Play();
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			particle.Play();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			particle.Stop();
		}

		#endregion Protected Methods
	}
}