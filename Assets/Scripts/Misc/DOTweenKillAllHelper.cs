using DG.Tweening;
using UnityEngine;

namespace StealthTD
{
	public class DOTweenKillAllHelper : MonoBehaviour
	{
		#region Private Methods

		private void OnDestroy()
		{
			int tweensKilled = DOTween.KillAll();
			Debug.Log($"DOTweenKillAllHelper killed {tweensKilled} tweens.");
		}

		#endregion Private Methods
	}
}