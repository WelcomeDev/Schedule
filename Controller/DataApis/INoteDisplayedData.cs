using System;

namespace Controller.DataApis
{
	public interface INoteDisplayedData
	{
		public string Name { get; set; }

		public string Phone { get; set; }

		public DateTime Date { get; set; }
	}
}
