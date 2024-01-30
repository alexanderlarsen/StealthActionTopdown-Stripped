using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StealthTD.UI
{
	public class CursorController : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		private Image arrow;

		[SerializeField]
		private Image crosshair;

		[SerializeField, ReadOnly]
		private GameObject active;

		#endregion Private Fields

		#region Public Methods

		public void SetArrowPointer()
		{
			arrow.gameObject.SetActive(true);
			crosshair.gameObject.SetActive(false);
			active = arrow.gameObject;
		}

		public void SetCrosshair()
		{
			arrow.gameObject.SetActive(false);
			crosshair.gameObject.SetActive(true);
			active = crosshair.gameObject;
		}

		#endregion Public Methods

		#region Private Methods

		private void Update()
		{
			if (Cursor.visible)
				Cursor.visible = false;

			if (Cursor.lockState != CursorLockMode.Confined)
				Cursor.lockState = CursorLockMode.Confined;

			if (active == null)
				return;

			active.transform.position = Mouse.current.position.value;
		}

		#endregion Private Methods
	}
}