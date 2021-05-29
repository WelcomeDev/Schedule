using System;
using System.ComponentModel;

namespace Controller.DataApis
{
	/// <summary>
	/// Интерфейс для связи Model и View, содержит отображаемые данные <see cref="CustomerNote"/>
	/// </summary>
	public interface INoteDisplayedData : INotifyPropertyChanged, IEquatable<INoteDisplayedData>
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }
	}
}
