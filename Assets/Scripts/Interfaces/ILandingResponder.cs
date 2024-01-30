namespace StealthTD.Interfaces
{
	/// <summary>
	/// Used for things that the player can land on, triggering a response.
	/// </summary>
	public interface ILandingResponder
	{
		void OnPlayerLand();
	} 
}