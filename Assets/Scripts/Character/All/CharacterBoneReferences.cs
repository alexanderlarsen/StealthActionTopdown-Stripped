using UnityEngine;

namespace StealthTD.Character
{
	public class CharacterBoneReferences : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public Transform Head { get; private set; }

		[field: SerializeField]
		public Transform Eyes { get; private set; }

		[field: SerializeField]
		public Transform LeftHand { get; private set; }

		[field: SerializeField]
		public Transform RightHand { get; private set; }

		#endregion Public Properties
	}
}