using Controller.DataApis;
using Model;
using Model.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
	public class MainController
	{
		private readonly NotesProvider dataProvider;
		private readonly Adapter adapter;

		private IEnumerable<CustomerNote> res;

		public MainController(Action<string> notifier)
		{
			dataProvider = NotesProvider.GetInstance();
			adapter = new Adapter();
		}

		/// <summary>
		/// Returns all data
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<INoteDisplayedData>> GetDisplayedDataAsync()
		{
			if (res is null)
				res = await dataProvider.GetAllAsync();

			return res.Select(x => adapter.ConvertToNoteData(x));
		}

		public async Task<IEnumerable<INoteDisplayedData>> GetDisplayedDataAsync(DateTime date)
		{
			if (res is null)
				res = await dataProvider.GetAllAsync();

			return res.Where(x => x.Date == date)
						.Select(x => adapter.ConvertToNoteData(x));
		}

		public async Task<IEnumerable<INoteDisplayedData>> GetDisplayedDataAsync(DateTime initialDate, DateTime finalDate)
		{
			if (res is null)
				res = await dataProvider.GetAllAsync();

			DatesToCorrectComparableFormat(ref initialDate, ref finalDate);

			return res.Where(x => x.Date >= initialDate && x.Date <= finalDate)
						.Select(x => adapter.ConvertToNoteData(x));
		}


		public INoteDisplayedData AddNewNote()
			=>  new NoteDisplayedData(new CustomerNote(null, null, DateTime.Today));

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
	}
}
