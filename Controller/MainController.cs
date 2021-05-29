using Controller.DataApis;
using Controller.Helper;
using Model;
using Model.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller
{
	public partial class MainController : ControllerBase
	{
		private const string DBSuccessfullySyncedMessage = "Успешно синхронизировано";
		private const string DBSynceErrorMessage = "Ошибка синхронизиронизации";

		public MainController(INotify notifier) : base(notifier)
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
			await Task.Run(() => Init());

			return reserve.Select(x => adapter.ConvertToNoteData(x));
		}

		public async Task<IEnumerable<INoteDisplayedData>> GetDisplayedDataAsync(DateTime date)
		{
			await Task.Run(() => Init());

			return reserve.Where(x => x.Date.EqualDates(date))
						.Select(x => adapter.ConvertToNoteData(x));
		}

		public async Task<IEnumerable<INoteDisplayedData>> GetDisplayedDataAsync(DateTime initialDate, DateTime finalDate)
		{
			await Task.Run(() => Init());

			finalDate = finalDate.ToEndOfTheDay();

			return reserve.Where(x => x.Date >= initialDate && x.Date <= finalDate)
				.Select(x => adapter.ConvertToNoteData(x));
		}

		public INoteDisplayedData AddNewNote()
			=> new NoteDisplayedData(new CustomerNote(null, null, DateTime.Today));


		public void Save()
		{
			var saveTask = dataProvider.SaveAsync();

			saveTask.ContinueWith(t => Notify(DBSuccessfullySyncedMessage),
				TaskContinuationOptions.OnlyOnRanToCompletion);

			saveTask.ContinueWith(t => Notify(DBSynceErrorMessage),
				TaskContinuationOptions.OnlyOnFaulted);
		}

		public void Remove(INoteDisplayedData obj)
		{
			var note = adapter.ConvertToNote(obj) as CustomerNote;
			dataProvider.Delete(note);
			reserve.Remove(note);

			Save();
		}

		//SOLVE: make notificator
		//TODO: add DatePicker Template
		//TODO: потокобезопасность в MainWindowData
		//SOLVE: при addAsync кидается Exception - нужно, чтобы Task просто умирал

		/// <summary>
		/// Добавляет запись асинхронно
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>Может вернуть Task.Status = Faulted, если возникла ошибка</returns>
		public async Task Add(INoteDisplayedData obj)
		{
			var note = adapter.ConvertToNote(obj) as CustomerNote;

			var addTask = dataProvider.AddAsync(note);

			await addTask.ContinueWith(t =>
							{
								Notify(OnSuccessfulAdditionMessage);
								reserve.Add(note);
								Save();
							},
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
