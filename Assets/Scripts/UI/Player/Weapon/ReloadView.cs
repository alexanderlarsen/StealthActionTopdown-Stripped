namespace StealthTD.UI.Player
{
	public class ReloadView : ViewBase
	{
		public void UpdateView(bool shouldReload)
		{
			SetVisibility(shouldReload, true, 0.2f, 0.2f);
		}
	}
}