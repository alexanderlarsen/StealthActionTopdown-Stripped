using UnityEngine;

namespace StealthTD.VFX
{
	public interface IImpactVfx
	{
		void SetRotation(Vector3 hitPosition, Vector3 hitNormal);
	}
}