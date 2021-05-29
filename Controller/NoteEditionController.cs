using System;
using System.Collections;
using System.Collections.Generic;

namespace Controller
{
	public class NoteEditionController : ControllerBase
	{
		private const int MinuteStep = 15;
		private const int MaxMinute = 60;

		public NoteEditionController(Action<string> notifier) : base(notifier)
		{ }

		public IEnumerable GetHours()
		{
			var hours = new List<string>(BL.Rules.ClosingHour - BL.Rules.OpeningHour);

			for (int i = BL.Rules.OpeningHour; i < BL.Rules.ClosingHour; i++)
			{
				hours.Add(InitTime(i));
			}

			return hours;
		}

		/// <summary>
		/// Инициализирует число в формате строки в виде "01", "02"...
		/// </summary>
		/// <param name="timeItem"></param>
		/// <returns></returns>
		public string InitTime(int timeItem)
		{
			if (timeItem < 10)
				return "0" + timeItem.ToString();
			return timeItem.ToString();
		}

		public IEnumerable GetMinute()
		{
			var minutes = new List<string>(MaxMinute / MinuteStep);

			for (int i = 0; i < MaxMinute; i += MinuteStep)
			{
				minutes.Add(InitTime(i));
			}

			return minutes;
		}

		public DateTime DateToCorrentFormat(DateTime time)
		{
			if (time.Hour < BL.Rules.OpeningHour)
			{
				time = time.AddHours(-time.Hour);
				time = time.AddHours(BL.Rules.OpeningHour);
			}
			else if (time.Hour >= BL.Rules.ClosingHour)
			{
				time = time.AddHours(-time.Hour);
				time = time.AddHours(BL.Rules.ClosingHour - 1);
			}

			return time;
		}
	}
}
