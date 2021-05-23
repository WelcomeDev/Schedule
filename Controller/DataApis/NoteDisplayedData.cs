using Model;
using System;

namespace Controller.DataApis
{
	public struct NoteDisplayedData : INoteDisplayedData
	{
		private readonly INote note;

		public string Name { get => note.Name; set => note.Name = value; }

		public string Phone { get => note.Phone; set => note.Phone = value; }

		public DateTime Date { get => note.Date; set => note.Date = value; }

		internal NoteDisplayedData(INote note)
		{
			this.note = note;
		}
	}
}
