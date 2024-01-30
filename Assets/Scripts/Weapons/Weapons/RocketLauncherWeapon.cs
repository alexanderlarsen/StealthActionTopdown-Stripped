using StealthTD.ObjectPool;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class RocketLauncherWeapon : ShootingWeapon
	{
		#region Private Fields

		[Inject]
		private readonly PrefabManager prefabManager;

		[SerializeField]
		private GameObject projectilePlaceholder;

		[SerializeField]
		private Transform projectileTip;

		#endregion Private Fields

		#region Public Properties

		public override Transform MuzzleOut => projectilePlaceholder.activeSelf ? projectileTip : muzzleOut;

		#endregion Public Properties

		#region Public Methods

		public override void UseWeapon()
		{
			base.UseWeapon();
			Fire(GetRpgRocket);
			projectilePlaceholder.SetActive(false);
		}

		public override void RefillAmmo()
		{
			base.RefillAmmo();
			projectilePlaceholder.SetActive(true);
		}

		#endregion Public Methods

		#region Private Methods

		private GameObject GetRpgRocket()
		{
			return prefabManager
				.Get<RpgRocket>(projectilePlaceholder.transform.position, projectilePlaceholder.transform.rotation)
				.SetDamage(Damage)
				.SetSoundOwner(SoundOwner)
				.SetLayerMask(impactLayerMask).gameObject;
		}

		#endregion Private Methods
	}
}