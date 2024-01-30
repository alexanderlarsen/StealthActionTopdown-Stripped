using StealthTD.Audio;
using StealthTD.Interfaces;
using StealthTD.ObjectPool;
using StealthTD.VFX;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class Bullet : PooledObject<Bullet>
	{
		#region Private Fields

		[Inject]
		private readonly VfxManager vfxManager;

		[Inject]
		private readonly AudioManager audioManager;

		[SerializeField]
		private float moveSpeed = 5;

		[SerializeField]
		private LayerMask layerMask;

		private Vector3 previousPosition;

		private int damage;

		#endregion Private Fields

		#region Public Properties

		public Transform SoundOwner { get; set; }

		#endregion Public Properties

		#region Public Methods

		public Bullet SetSoundOwner(Transform soundOwner)
		{
			SoundOwner = soundOwner;
			return this;
		}

		public Bullet SetLayerMask(LayerMask layerMask)
		{
			this.layerMask = layerMask;
			return this;
		}

		public Bullet SetDamage(int damage)
		{
			this.damage = damage;
			return this;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			var colliders = Physics.OverlapSphere(transform.position, 0.1f, layerMask);

			foreach (var collider in colliders)
				BulletHit(collider, transform.position, -transform.forward);

			if (colliders.Length > 0)
			{
				ReturnToObjectPool();
				return;
			}

			previousPosition = transform.position;
		}

		#endregion Protected Methods

		#region Private Methods

		private void FixedUpdate()
		{
			Move();
			CheckAndHandleImpact();
		}

		private void Move()
		{
			previousPosition = transform.position;
			transform.position += moveSpeed * Time.fixedDeltaTime * transform.forward;
		}

		private void CheckAndHandleImpact()
		{
			if (Physics.Raycast(previousPosition, transform.forward, out RaycastHit hit, moveSpeed * Time.fixedDeltaTime, layerMask))
			{
				BulletHit(hit.collider, hit.point, hit.normal);
				ReturnToObjectPool();
			}
		}

		private void BulletHit(Collider collider, Vector3 hitPoint, Vector3 hitNormal)
		{
			SurfaceType surfaceType = SurfaceType.Concrete;

			if (collider.TryGetComponent(out IHitable hitable))
				hitable.Hit(hitPoint, hitNormal);

			if (collider.TryGetComponent(out ISurface surface))
				surfaceType = surface.Type;

			if (collider.TryGetComponent(out IDamagable damageable))
				damageable.TakeDamage(damage);

			audioManager.ImpactAudio.PlayImpactAudio(surfaceType, hitPoint, SoundOwner);
			vfxManager.DisplayImpactVfx(surfaceType, hitPoint, hitNormal);
		}

		#endregion Private Methods
	}
}