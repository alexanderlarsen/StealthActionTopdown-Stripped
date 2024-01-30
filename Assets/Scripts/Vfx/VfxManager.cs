using StealthTD.ObjectPool;
using UnityEngine;
using Zenject;

namespace StealthTD.VFX
{
	public class VfxManager
	{
		#region Private Fields

		[Inject]
		private readonly PrefabManager prefabManager;

		#endregion Private Fields

		#region Public Methods

		public void DisplayImpactVfx(SurfaceType surfaceMaterial, Vector3 hitPosition, Vector3 hitNormal)
		{
			IImpactVfx impact = surfaceMaterial switch
			{
				SurfaceType.Concrete => prefabManager.Get<ImpactConcreteVfx>(hitPosition),
				SurfaceType.Body => prefabManager.Get<ImpactBloodSpatterVfx>(hitPosition),
				_ => null
			};

			impact.SetRotation(hitPosition, hitNormal);
		}

		public ExplosionVfx DisplayExplosion(Vector3 position)
		{
			return prefabManager.Get<ExplosionVfx>(position);
		}

		public SleepingVfx DisplaySleepingVfx(Transform parent)
		{
			SleepingVfx sleepingVfx = prefabManager.Get<SleepingVfx>();
			GameObject instance = sleepingVfx.gameObject;
			instance.transform.SetParent(parent);
			instance.transform.localPosition = new Vector3(0.0199999996f, 0.451999992f, 1.04999995f);
			instance.transform.localRotation = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
			instance.transform.localScale = new Vector3(0.600000024f, 0.600000024f, 0.600000024f);
			return sleepingVfx;
		}

		public BloodPoolVfx DisplayBloodPoolVfx(Transform parent)
		{
			BloodPoolVfx bloodPoolVfx = prefabManager.Get<BloodPoolVfx>();
			GameObject instance = bloodPoolVfx.gameObject;
			instance.transform.SetParent(parent);
			instance.transform.localPosition = new Vector3(0.109999999f, 0.0130000003f, 0.540000021f);
			instance.transform.localRotation = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
			instance.transform.localScale = Vector3.one;
			return bloodPoolVfx;
		}

		#endregion Public Methods
	}
}