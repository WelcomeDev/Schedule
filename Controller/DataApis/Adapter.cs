﻿using Model;

namespace Controller.DataApis
{
	/// <summary>
	/// Паттерн адаптер - преобразует один интерфейс в другой
	/// </summary>
	public class Adapter
	{
		public INoteDisplayedData ConvertToNoteData(INote customerNote) => new NoteDisplayedData(customerNote);
	}
}
