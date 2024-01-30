using StealthTD.Audio;
using StealthTD.Interfaces;
using StealthTD.VFX;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class MeleeWeapon : Weapon
	{
		#region Private Fields

		[Inject]
		private readonly VfxManager vfxManager;

		[Inject]
		private readonly AudioManager audioManager;

		[SerializeField]
		private Transform impactOrigin;

		[SerializeField]
		private Transform weaponTip;

		#endregion Private Fields

		#region Public Methods

		public void DealDamage()
		{
			foreach (Collider collider in Physics.OverlapSphere(impactOrigin.position, 0.4f, impactLayerMask))
			{
				SurfaceType surfaceType = SurfaceType.Concrete;

				if (collider.TryGetComponent(out IHitable hitable))
					hitable.Hit(weaponTip.position, -weaponTip.forward);

				if (collider.TryGetComponent(out ISurface surface))
					surfaceType = surface.Type;

				if (collider.TryGetComponent(out IDamagable damagable))
					damagable.TakeDamage(Damage);

				audioManager.ImpactAudio.PlayImpactAudio(surfaceType, weaponTip.position, SoundOwner);

				if (surfaceType == SurfaceType.Body)
					vfxManager.DisplayImpactVfx(surfaceType, weaponTip.position, -weaponTip.forward);
			}
		}

		public override bool IsClippingIntoWall()
		{
			return false;
		}

		#endregion Public Methods
	}
}