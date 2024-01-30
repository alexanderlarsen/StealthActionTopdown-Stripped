using StealthTD.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace StealthTD.Weapons
{
	public abstract class Weapon : MonoBehaviour, IEquipable
	{
		#region Protected Fields

		protected Transform characterRoot;

		[SerializeField, ReadOnly]
		protected LayerMask impactLayerMask;

		#endregion Protected Fields

		#region Private Fields

		[SerializeField]
		private Transform modelTransform;

		[SerializeField]
		private AudioClip equipSound;

		private Coroutine fallDownRoutine;
		private Coroutine cooldownRoutine;
		private Vector3 modelInitialLocalPosition;

		#endregion Private Fields

		#region Public Events

		public event Action OnEquipped;

		#endregion Public Events

		#region Public Properties

		[field: SerializeField]
		public AnimatorLayer AnimatorLayer { get; private set; }

		[field: SerializeField]
		public string WeaponName { get; private set; }

		[field: SerializeField]
		public float Cooldown { get; private set; }

		[field: SerializeField]
		public int Damage { get; private set; }

		[field: SerializeField, ReadOnly]
		public bool IsCoolingDown { get; private set; }

		[field: SerializeField, ReadOnly]
		public bool IsEquipped { get; private set; }

		public Transform EquipableTransform => transform;

		public string EquipableName => WeaponName;

		public Transform SoundOwner { get; set; }

		#endregion Public Properties

		#region Public Methods

		public virtual void UseWeapon()
		{
			if (Cooldown <= 0)
				return;

			if (cooldownRoutine != null)
				StopCoroutine(cooldownRoutine);

			cooldownRoutine = StartCoroutine(CooldownRoutine());
		}

		public virtual void StopUsingWeapon()
		{
		}

		public abstract bool IsClippingIntoWall();

		public virtual void Equip(Transform characterRoot, Transform handTransform)
		{
			StopFallDownRoutine();
			this.characterRoot = characterRoot;
			AudioSource.PlayClipAtPoint(equipSound, transform.position);
			transform.parent = handTransform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			modelTransform.localPosition = Vector3.zero;
			IsEquipped = true;
			OnEquipped?.Invoke();
		}

		public virtual void Unequip()
		{
			StopAllCoroutines();
			StopUsingWeapon();
			characterRoot = null;
			StartFallDownRoutine();
			transform.parent = null;
			transform.localRotation = Quaternion.identity;
			modelTransform.localPosition = modelInitialLocalPosition;
			IsEquipped = false;
			IsCoolingDown = false;
		}

		public void SetImpactLayerMask(LayerMask layerMask)
		{
			impactLayerMask = layerMask;
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void Awake()
		{
			modelInitialLocalPosition = modelTransform.localPosition;
		}

		protected virtual void Start()
		{
			if (!IsEquipped)
				StartFallDownRoutine();
		}

		protected virtual void Update()
		{
			if (!IsEquipped)
				transform.Rotate(Vector3.up, Time.deltaTime * 50f);
		}

		#endregion Protected Methods

		#region Private Methods

		private void StartFallDownRoutine()
		{
			StopFallDownRoutine();
			fallDownRoutine = StartCoroutine(FallDownRoutine());
		}

		private void StopFallDownRoutine()
		{
			if (fallDownRoutine != null)
				StopCoroutine(fallDownRoutine);
		}

		private IEnumerator CooldownRoutine()
		{
			IsCoolingDown = true;
			yield return new WaitForSeconds(Cooldown);
			IsCoolingDown = false;
		}

		private IEnumerator FallDownRoutine()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100, LayerMask.GetMask("Ground", "Environment")))
			{
				Vector3 targetPosition = hit.point + Vector3.up;

				while (transform.position != targetPosition)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, 9.81f * Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
			}
			else Debug.LogError("Could not find the ground within 100 meters of the weapon.");
		}

		#endregion Private Methods
	}
}