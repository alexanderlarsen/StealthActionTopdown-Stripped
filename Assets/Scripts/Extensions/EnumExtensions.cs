using System;

namespace StealthTD.Extensions
{
	public static class EnumExtensions
	{
		#region Public Methods

		public static int ToInt<T>(this T enumValue) where T : Enum
		{
			return Convert.ToInt32(enumValue);
		}

		#endregion Public Methods
	}
}