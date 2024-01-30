using UnityEngine;

namespace StealthTD.FSM
{
	public abstract class State : MonoBehaviour
	{
		#region Protected Properties

		protected bool IsPaused => Time.timeScale == 0;

		#endregion Protected Properties

		#region Protected Methods

		protected virtual void Enter()
		{ }

		protected virtual void Exit()
		{ }

		#endregion Protected Methods

		#region Private Methods

		private void OnEnable()
		{
			Enter();
		}

		private void OnDisable()
		{
			Exit();
		}

		#endregion Private Methods
	}
}