using DG.Tweening;
using TMPro;
using UnityEngine;

namespace StealthTD.UI.InGame
{
	public class ObjectivesView : ViewBase
	{
		#region Public Properties

		[field: SerializeField]
		public TextMeshProUGUI GetAllKeycardsTmp { get; private set; }

		[field: SerializeField]
		public TextMeshProUGUI KillAllEnemiesTmp { get; private set; }

		[field: SerializeField]
		public TextMeshProUGUI NoDetectionTmp { get; private set; }

		[field: SerializeField]
		public TextMeshProUGUI ReachGoalTmp { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void UpdateView(
			bool getAllKeycardsEnabled,
			bool killAllEnemiesEnabled,
			bool noDetectionEnabled,
			bool allObjectivesCompleted,
			int currentKeycardCount,
			int totalKeycardCount,
			int currentEnemyCount,
			int totalEnemyCount,
			bool wasDetected,
			bool isLevelCompleted)
		{
			AnimateVisibility(GetAllKeycardsTmp.gameObject, getAllKeycardsEnabled);
			AnimateVisibility(KillAllEnemiesTmp.gameObject, killAllEnemiesEnabled);
			AnimateVisibility(NoDetectionTmp.gameObject, noDetectionEnabled);
			AnimateVisibility(ReachGoalTmp.gameObject, allObjectivesCompleted);

			if (getAllKeycardsEnabled)
			{
				GetAllKeycardsTmp.text = $"Collect all keycards ({currentKeycardCount}/{totalKeycardCount})";

				if (currentKeycardCount == totalKeycardCount)
				{
					GetAllKeycardsTmp.color = Color.green;
					GetAllKeycardsTmp.text = $"<s>{GetAllKeycardsTmp.text}</s>";
				}
			}

			if (killAllEnemiesEnabled)
			{
				KillAllEnemiesTmp.text = $"Kill all enemies ({currentEnemyCount}/{totalEnemyCount})";

				if (currentEnemyCount == totalEnemyCount)
				{
					KillAllEnemiesTmp.color = Color.green;
					KillAllEnemiesTmp.text = $"<s>{KillAllEnemiesTmp.text}</s>";
				}
			}

			if (wasDetected && noDetectionEnabled)
				NoDetectionTmp.color = Color.red;

			if (isLevelCompleted)
			{
				if (noDetectionEnabled)
				{
					NoDetectionTmp.color = Color.green;
					NoDetectionTmp.text = $"<s>{NoDetectionTmp.text}</s>";
				}

				ReachGoalTmp.color = Color.green;
				ReachGoalTmp.text = $"<s>{ReachGoalTmp.text}</s>";
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void AnimateVisibility(GameObject gameObj, bool enable)
		{
			if (enable && !gameObj.activeSelf)
			{
				gameObj.transform.localScale = Vector3.zero;
				gameObj.SetActive(true);
			}

			gameObj.transform
				.DOScale(enable ? 1 : 0, 0.2f)
				.SetId($"{gameObj.GetHashCode()}_ObjectivesView_SetGameObjectVisibility")
				.OnComplete(() =>
				{
					if (!enable)
						gameObj.SetActive(false);
				});
		}

		#endregion Private Methods
	}
}