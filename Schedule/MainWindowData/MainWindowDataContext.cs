using Controller.DataApis;
using Schedule.GUIs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
			public ObservableCollection<NoteItem> DisplayedData { get; } = new ObservableCollection<NoteItem>();

			public MainWindowData()
			{
				allViewItems = new List<NoteItem>();
			}

			public void InitSource(IEnumerable<INoteDisplayedData> result)
			{
				if (allViewItems.Count > 0)
				{
					allViewItems.Clear();
					DisplayedData.Clear();
				}

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

				DisplayedData.Clear();
				foreach (var item in res)
				{
					DisplayedData.Add(item);
				}
			}
		}
	}
}
