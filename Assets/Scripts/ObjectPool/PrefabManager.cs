using StealthTD.VFX;
using StealthTD.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StealthTD.ObjectPool
{
	public class PrefabManager : MonoBehaviour
	{
		#region Private Fields

		private readonly Dictionary<Type, IPool> pools = new();

		[Header("Projectiles")]
		[SerializeField]
		private GameObject bulletPrefab;

		[SerializeField]
		private GameObject rpgRocketPrefab;

		[Header("VFX")]
		[SerializeField]
		private GameObject impactConcretePrefab;

		[SerializeField]
		private GameObject impactBloodSpatterPrefab;

		[SerializeField]
		private GameObject sleepingPrefab;

		[SerializeField]
		private GameObject bloodPoolPrefab;

		[SerializeField]
		private GameObject explosionPrefab;

		#endregion Private Fields

		#region Public Methods

		public T Get<T>() where T : Component
		{
			return GetPool<T>().Get();
		}

		public T Get<T>(Vector3 position) where T : Component
		{
			return GetPool<T>().Get(position);
		}

		public T Get<T>(Vector3 position, Quaternion rotation) where T : Component
		{
			return GetPool<T>().Get(position, rotation);
		}

		public T Get<T>(Vector3 position, Quaternion rotation, Transform parent) where T : Component
		{
			return GetPool<T>().Get(position, rotation, parent);
		}

		public void Return<T>(GameObject gameObj) where T : Component
		{
			GetPool<T>().Return(gameObj);
		}

		/// <summary>
		/// Remember to cancel the coroutine yourself.
		/// </summary>
		public Coroutine ReturnDelayed<T>(GameObject gameObject, float delay) where T : Component
		{
			return StartCoroutine(ReturnDelayedRoutine<T>(gameObject, delay));
		}

		public void CancelReturnDelayed(Coroutine coroutine)
		{
			if (coroutine != null)
				StopCoroutine(coroutine);
		}

		#endregion Public Methods

		#region Private Methods

		private IEnumerator ReturnDelayedRoutine<T>(GameObject gameObject, float delay) where T : Component
		{
			yield return new WaitForSeconds(delay);
			Return<T>(gameObject);
		}

		private ObjectPool<T> GetPool<T>() where T : Component
		{
			if (pools.TryGetValue(typeof(T), out IPool value) && value is ObjectPool<T> pool)
				return pool;
			else
				throw new InvalidOperationException($"Could not find object pool of type '{typeof(T)}'.");
		}

		private void Start()
		{
			pools.Add(typeof(Bullet),
				new ObjectPool<Bullet>(bulletPrefab, true, -1, 30, transform));

			pools.Add(typeof(RpgRocket),
				new ObjectPool<RpgRocket>(rpgRocketPrefab, true, -1, 3, transform));

			pools.Add(typeof(ImpactConcreteVfx),
				new ObjectPool<ImpactConcreteVfx>(impactConcretePrefab, false, 10, 10, transform));

			pools.Add(typeof(ImpactBloodSpatterVfx),
				new ObjectPool<ImpactBloodSpatterVfx>(impactBloodSpatterPrefab, false, 10, 10, transform));

			pools.Add(typeof(SleepingVfx),
				new ObjectPool<SleepingVfx>(sleepingPrefab, true, 3, 3, transform));

			pools.Add(typeof(BloodPoolVfx),
				new ObjectPool<BloodPoolVfx>(bloodPoolPrefab, false, 10, 10, transform));

			pools.Add(typeof(ExplosionVfx),
				new ObjectPool<ExplosionVfx>(explosionPrefab, true, -1, 5, transform));
		}

		#endregion Private Methods
	}
}