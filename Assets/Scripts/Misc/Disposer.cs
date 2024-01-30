using UnityEngine;

namespace StealthTD.Helpers
{
	public class Disposer : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		private Component[] components;

		[SerializeField]
		private GameObject[] gameObjects;

		#endregion Private Fields

		#region Public Methods

		public void DisposeAll()
		{
			foreach (Component component in components)
				if (component != null)
					Destroy(component);

			foreach (GameObject gameObj in gameObjects)
				if (gameObj != null)
					Destroy(gameObj);

			if (this != null)
				Destroy(this);
		}

		#endregion Public Methods
	}
}