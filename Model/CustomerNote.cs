﻿using System;

namespace Model
{
	[Serializable]
	public class CustomerNote : INote
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }

		public CustomerNote(string name, string phone, DateTime date)
		{

		}
	}
}
