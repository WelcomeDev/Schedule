using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Helper
{
	/// <summary>
	/// Методы расширения для работы с DateTime
	/// </summary>
	internal static class DateHelper
	{
		public static bool EqualDates(this DateTime first, DateTime second)
			=> first.Year == second.Year &&
					first.Month == second.Month &&
					first.Day == second.Day;

		public static DateTime ToEndOfTheDay(this DateTime date)
			=> date.AddHours(23 - date.Hour)
					.AddSeconds(59 - date.Second)
					.AddMinutes(59 - date.Minute);
	}
}
