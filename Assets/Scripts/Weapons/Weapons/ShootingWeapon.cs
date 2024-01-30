using System;
using UnityEngine;

namespace StealthTD.Weapons
{
	public abstract class ShootingWeapon : Weapon
	{
		#region Protected Fields

		[SerializeField]
		protected Transform muzzleOut;

		[SerializeField]
		protected Silencer silencer;

		#endregion Protected Fields

		#region Private Fields

		[SerializeField]
		private GameObject muzzleFlare;

		[SerializeField]
		private AudioClip fireAudio;

		[SerializeField]
		private AudioClip reloadAudio;

		[SerializeField]
		private AudioClip dryFireAudio;

		private LayerMask environmentLayerMask;

		#endregion Private Fields

		#region Public Events

		public event Action OnCurrentAmmoChanged;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField]
		public int MaxAmmo { get; private set; }

		[field: SerializeField, ReadOnly]
		public int CurrentAmmo { get; protected set; }

		public virtual Transform MuzzleOut => IsSilenced ? silencer.MuzzleOut : muzzleOut;

		#endregion Public Properties

		#region Private Properties

		private bool IsSilenced => silencer != null && silencer.gameObject.activeInHierarchy;

		#endregion Private Properties

		#region Public Methods

		public override bool IsClippingIntoWall()
		{
			float distance = Vector3.Distance(transform.position, MuzzleOut.position);
			return Physics.Raycast(transform.position, characterRoot.forward, distance, environmentLayerMask);
		}

		public virtual void RefillAmmo()
		{
			AudioSource.PlayClipAtPoint(reloadAudio, transform.position);
			CurrentAmmo = MaxAmmo;
			OnCurrentAmmoChanged?.Invoke();
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void Awake()
		{
			base.Awake();
			environmentLayerMask = LayerMask.GetMask("Environment", "Ground");
		}

		protected virtual void Fire(Func<GameObject> getProjectile)
		{
			if (CurrentAmmo <= 0)
			{
				AudioSource.PlayClipAtPoint(dryFireAudio, transform.position);
				return;
			}

			AudioSource.PlayClipAtPoint(IsSilenced ? silencer.FireAudio : fireAudio, transform.position);
			getProjectile.Invoke();

			if (muzzleFlare != null)
			{
				muzzleFlare.transform.position = IsSilenced ? silencer.MuzzleOut.position : muzzleOut.position;
				muzzleFlare.SetActive(false);
				muzzleFlare.SetActive(true);
			}

			CurrentAmmo--;
			OnCurrentAmmoChanged?.Invoke();
		}

		protected override void Start()
		{
			base.Start();

			CurrentAmmo = MaxAmmo;
			OnCurrentAmmoChanged?.Invoke();

			if (muzzleFlare != null)
				muzzleFlare.SetActive(false);
		}

		#endregion Protected Methods
	}
}