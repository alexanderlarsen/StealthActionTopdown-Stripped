using StealthTD.Interfaces;
using TMPro;
using UnityEngine;

namespace StealthTD.UI.Player
{
	public class EquipView : ViewBase
	{
		[SerializeField]
		private TextMeshProUGUI equipObjectNameTmp;

		[SerializeField]
		private FollowWorldSpaceObject equipUiFollow;

		bool isVisible; 

		public void UpdateView(IEquipable equipable)
		{
			if (equipable != null && !isVisible)
			{
				isVisible = true;
				FadeIn(0.2f);
			}
			else if(equipable == null && isVisible)
			{
				isVisible = false;
				FadeOut(0.2f);
			}

			if (equipable == null)
				return;

			equipObjectNameTmp.text = equipable.EquipableName;
			equipUiFollow.SetTargetTransform(equipable.EquipableTransform);
		}
	}
}