using Controller.DataApis;
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

		public MainController(Action<string> notifier) : base(notifier)
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
			dataProvider.Delete(obj as CustomerNote);

			Save();
		}

		//SOLVE: fill min and hour CB
		//SOLVE: binding dates to TextBox
		//SOLVE: check save
		//SOLVE: make notificator
		//SOLVE: chars limit in input!
		//TODO: потокобезопасность в MainWindowData
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

			await addTask.ContinueWith(t =>
							{
								Notify(OnSuccessfulAdditionMessage);
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
