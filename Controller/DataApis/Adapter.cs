using Model;

namespace Controller.DataApis
{
	/// <summary>
	/// Паттерн адаптер - преобразует один интерфейс в другой
	/// </summary>
	public class Adapter
	{
		public INoteDisplayedData ConvertNoteTo(INote customerNote) => new NoteDisplayedData(customerNote);
	}
}
