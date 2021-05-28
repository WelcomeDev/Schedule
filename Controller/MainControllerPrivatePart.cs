using Controller.DataApis;
using Model;
using Model.DataProviders;
using System;
using System.Collections.Generic;

namespace Controller
{
	public partial class MainController
	{
		private readonly NotesProvider dataProvider;
		private readonly Adapter adapter;
		private IEnumerable<CustomerNote> res;

		private void DatesToCorrectComparableFormat(ref DateTime initialDate, ref DateTime finalDate)
		{
			//в начало суток
			initialDate.AddHours(-initialDate.Hour)
					.AddSeconds(-initialDate.Second)
					.AddMinutes(-initialDate.Minute);

			//в самый конец суток
			finalDate.AddHours(24 - finalDate.Hour)
					.AddSeconds(59 - finalDate.Second)
					.AddMinutes(59 - finalDate.Minute);
		}

		private const string OnSuccessfulAdditionMessage = "Запись успешно добавлена";
		private const string OnAdditionExceptionMessage = "Во время добавления возникла ошибка!";
	}
}
