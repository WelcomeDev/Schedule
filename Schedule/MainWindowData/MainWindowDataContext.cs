using Controller.DataApis;
using Schedule.GUIs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Schedule
{
	/// <summary>
	/// Часть, где содержится класс, управляющий данными отображения MainWindow
	/// Создан как объект для биндинга в MainWindow.DataContext
	/// </summary>
	public partial class MainWindow
	{
		private class MainWindowData
		{
			private readonly List<NoteItem> allViewItems;

			public ObservableCollection<NoteItem> DisplayedData { get; private set; }

			public MainWindowData()
			{
				allViewItems = new List<NoteItem>();
			}

			public void InitSource(IEnumerable<INoteDisplayedData> result)
			{
				DisplayedData = new ObservableCollection<NoteItem>();

				foreach (var dat in result)
				{
					var note = new NoteItem(dat);

					allViewItems.Add(note);
					DisplayedData.Add(note);
				}
			}

			public void Remove(INoteDisplayedData noteDisplayedData)
			{
				var itemToBeRemoved = allViewItems.FirstOrDefault(x => x.NoteData.Equals(noteDisplayedData));

				allViewItems.Remove(itemToBeRemoved);
				DisplayedData.Remove(itemToBeRemoved);
			}

			public void Add(INoteDisplayedData noteDisplayedData)
			{
				var noteUI = new NoteItem(noteDisplayedData);
				allViewItems.Add(noteUI);
				DisplayedData.Add(noteUI);
			}

			public void DisplayDates(IEnumerable<INoteDisplayedData> newDisplay)
			{
				var res = allViewItems.Where(x => x.NoteData.Equals(newDisplay));

				DisplayedData = new ObservableCollection<NoteItem>(res);
			}
		}
	}
}
