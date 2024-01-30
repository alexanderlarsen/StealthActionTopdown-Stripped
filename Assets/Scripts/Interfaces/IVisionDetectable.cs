using UnityEngine;

namespace StealthTD.Interfaces
{
	public interface IVisionDetectable
	{
		Transform Transform { get; }
		bool IsDetectable { get; }
	} 
}