using StealthTD.ObjectPool;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class SemiAutoShootingWeapon : ShootingWeapon
	{
		#region Private Fields

		[Inject]
		private readonly PrefabManager prefabManager;

		#endregion Private Fields

		#region Public Methods

		public override void UseWeapon()
		{
			base.UseWeapon();
			Fire(GetBullet);
		}

		#endregion Public Methods

		#region Private Methods

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