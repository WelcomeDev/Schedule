using Controller.DataApis;
using Model;
using Model.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
	public partial class MainController
	{
		public MainController(Action<string> notifier)
		{
			dataProvider = NotesProvider.GetInstance();
			adapter = new Adapter();
			this.notifier = notifier;
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
			=> new NoteDisplayedData(new CustomerNote(null, null, DateTime.Today));


		public void Remove(INoteDisplayedData obj)
		{
			dataProvider.Delete(obj as CustomerNote);
		}

		//TODO: потокобезопасность в MainWindowData
		//SOLVE: проверка имени через Regex
		//TODO: заблокировать ввод в NoteItem
		//SOLVE: проверить возможность редактирования в Page и открытии через NoteItem
		//TODO: проверка на цифры в имени
		//SOLVE: при addAsync кидается Exception - нужно, чтобы Task просто умирал

		/// <summary>
		/// Добавляет запись асинхронно
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>Может вернуть Task.Status = Faulted, если возникла ошибка</returns>
		public async Task Add(INoteDisplayedData obj)
		{
			var note = adapter.ConvertToNote(obj);

			var addTask = dataProvider.AddAsync(note as CustomerNote);

			await addTask.ContinueWith(t => Notify(OnSuccessfulAdditionMessage),
							TaskContinuationOptions.OnlyOnRanToCompletion);

			await addTask.ContinueWith(t =>
				{
					Notify(OnAdditionExceptionMessage);
					throw t.Exception.InnerException;
				},
				TaskContinuationOptions.OnlyOnFaulted);

		}
	}
}
