using Controller.DataApis;
using Model;
using Model.DataProviders;
using System.Collections.Generic;
using System.Linq;

namespace Controller
{
	public partial class MainController
	{
		private readonly NotesProvider dataProvider;
		private readonly Adapter adapter;
		private List<CustomerNote> reserve;

		private async void Init()
		{
			if (reserve is null)
				reserve = (await dataProvider.GetAllAsync()).ToList();
		}

		private const string OnSuccessfulAdditionMessage = "Запись успешно добавлена";
		private const string OnAdditionExceptionMessage = "Во время добавления возникла ошибка!";
	}
}
