using StealthTD.ObjectPool;
using System.Collections;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class FullAutoShootingWeapon : ShootingWeapon
	{
		#region Private Fields

		[Inject]
		private readonly PrefabManager prefabManager;

		private Coroutine fireRoutine;

		#endregion Private Fields

		#region Public Methods

		public override void UseWeapon()
		{
			base.UseWeapon();
			fireRoutine = StartCoroutine(FireContinuous());
		}

		public override void StopUsingWeapon()
		{
			base.StopUsingWeapon();

			if (fireRoutine != null)
				StopCoroutine(fireRoutine);
		}

		#endregion Public Methods

		#region Private Methods

		private IEnumerator FireContinuous()
		{
			while (true)
			{
				Fire(GetBullet);
				yield return new WaitForSeconds(Cooldown);
			}
		}

		private GameObject GetBullet()
		{
			return prefabManager
				.Get<Bullet>(MuzzleOut.position, MuzzleOut.rotation)
				.SetDamage(Damage)
				.SetSoundOwner(SoundOwner)
				.SetLayerMask(impactLayerMask).gameObject;
		}

		#endregion Private Methods
	}
}