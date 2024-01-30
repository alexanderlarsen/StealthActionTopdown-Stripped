using StealthTD.ObjectPool;
using System.Collections;
using UnityEngine;

namespace StealthTD.VFX
{
	public class BloodPoolVfx : PooledObject<BloodPoolVfx>
	{
		#region Private Fields

		[SerializeField]
		private ParticleSystem particle;

		private Coroutine handleAnimationRoutine;

		#endregion Private Fields

		#region Protected Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			particle.Play();
			handleAnimationRoutine = StartCoroutine(HandleAnimationRoutine());
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			particle.Stop();

			if (handleAnimationRoutine != null)
				StopCoroutine(handleAnimationRoutine);
		}

		#endregion Protected Methods

		#region Private Methods

		private IEnumerator HandleAnimationRoutine()
		{
			transform.Rotate(Vector3.forward, Random.Range(-180f, 180f));
			yield return new WaitForSeconds(particle.main.duration - 0.25f);
			particle.Pause();
		}

		#endregion Private Methods
	}
}