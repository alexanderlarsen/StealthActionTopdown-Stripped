using StealthTD.Audio;
using StealthTD.Extensions;
using StealthTD.ObjectPool;
using StealthTD.VFX;
using System.Linq;
using UnityEngine;
using Zenject;

namespace StealthTD.Weapons
{
	public class RpgRocket : PooledObject<RpgRocket>
	{
		#region Private Fields

		[Inject]
		private readonly AudioManager audioManager;

		[Inject]
		private readonly VfxManager vfxManager;

		[SerializeField]
		private Transform tipTransform;

		[SerializeField]
		private float explosionRadius;

		[SerializeField]
		private float moveSpeed = 15f;

		[SerializeField]
		private LayerMask explosionSoundLayerMask;

		private int damage = 250;
		private LayerMask layerMask;
		private Vector3 previousPosition;

		#endregion Private Fields

		#region Public Properties

		public Transform SoundOwner { get; set; }

		#endregion Public Properties

		#region Public Methods

		public RpgRocket SetSoundOwner(Transform soundOwner)
		{
			SoundOwner = soundOwner;
			return this;
		}

		public RpgRocket SetLayerMask(LayerMask layerMask)
		{
			this.layerMask = layerMask;
			return this;
		}

		public RpgRocket SetDamage(int damage)
		{
			this.damage = damage;
			return this;
		}

		#endregion Public Methods

		#region Protected Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			if (Physics.OverlapSphere(tipTransform.position, 0.1f, layerMask).Length > 0)
			{
				TriggerExplosion();
				return;
			}

			previousPosition = tipTransform.position;
		}

		#endregion Protected Methods

		#region Private Methods

		private void FixedUpdate()
		{
			Move();

			if (Physics.Raycast(previousPosition, transform.forward, moveSpeed * Time.fixedDeltaTime, layerMask))
				TriggerExplosion();
		}

		private void Move()
		{
			previousPosition = tipTransform.position;
			transform.position += moveSpeed * Time.fixedDeltaTime * transform.forward;
		}

		private void TriggerExplosion()
		{
			vfxManager.DisplayExplosion(tipTransform.position);
			audioManager.ImpactAudio.PlayExplosionAudio(tipTransform.position, SoundOwner);
			Physics.OverlapSphere(tipTransform.position, explosionRadius, layerMask).Where(col => !Physics.Linecast(transform.position, col.transform.position, LayerMask.GetMask("Environment"))).ToArray().FindDamagablesAndDealDamage(damage);
			ReturnToObjectPool();
		}

		#endregion Private Methods
	}
}