using UnityEngine;
using Zenject;

namespace StealthTD.ObjectPool
{
	public abstract class PooledObject<T> : MonoBehaviour where T : Component
	{
		#region Private Fields

		[Inject]
		private readonly PrefabManager prefabManager;

		[SerializeField]
		private bool autoReturnToPool;

		[SerializeField, Min(0)]
		private float returnToPoolDelay = 0;

		private Coroutine returnToPoolRoutine;

		#endregion Private Fields

		#region Public Methods

		public void ReturnToObjectPool()
		{
			if (autoReturnToPool)
				prefabManager.CancelReturnDelayed(returnToPoolRoutine);

			prefabManager.Return<T>(gameObject);
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void OnEnable()
		{
			if (autoReturnToPool)
				returnToPoolRoutine = prefabManager.ReturnDelayed<T>(gameObject, returnToPoolDelay);
		}

		protected virtual void OnDisable()
		{
			if (autoReturnToPool)
				prefabManager.CancelReturnDelayed(returnToPoolRoutine);
		}

		#endregion Protected Methods
	}
}