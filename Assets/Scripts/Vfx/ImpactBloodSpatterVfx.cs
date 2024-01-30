using StealthTD.ObjectPool;
using UnityEngine;

namespace StealthTD.VFX
{
	public class ImpactBloodSpatterVfx : PooledObject<ImpactBloodSpatterVfx>, IImpactVfx
	{
		#region Public Methods

		public void SetRotation(Vector3 hitPosition, Vector3 hitNormal)
		{
			transform.LookAt(hitPosition - hitNormal);
		}

		#endregion Public Methods
	}
}