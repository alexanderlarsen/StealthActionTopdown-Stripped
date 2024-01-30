using System.Linq;
using UnityEngine;

namespace StealthTD.Enemy.AI
{
	public class EnemyPatrolPath : MonoBehaviour
	{
		#region Private Fields

		private Vector3[] points;

		#endregion Private Fields

		#region Public Properties

		public int NumPoints => points.Length;

		#endregion Public Properties

		#region Public Methods

		public Vector3 GetPoint(int index)
		{
			return points[index];
		}

		public int FindNearestPatrolPointIndex(Vector3 currentPosition)
		{
			Vector3 closestPoint = points
				.OrderBy(point => Vector3.Distance(currentPosition, point))
				.FirstOrDefault();

			return points
				.ToList()
				.IndexOf(closestPoint);
		}

		#endregion Public Methods

		#region Private Methods

		private void Awake()
		{
			GetChildTransformPositions();
		}

		private void GetChildTransformPositions()
		{
			points = new Vector3[transform.childCount];

			for (int i = 0; i < transform.childCount; i++)
				points[i] = transform.GetChild(i).position;
		}

		#endregion Private Methods
	}
}