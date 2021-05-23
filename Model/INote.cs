using System;

namespace Model
{
	public interface INote
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }
	}
}
