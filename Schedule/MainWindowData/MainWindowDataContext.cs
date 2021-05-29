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
		private class MainWindowData : INotifyPropertyChanged
		{
			private readonly List<NoteItem> allViewItems;
			private string datesRange;

			public event PropertyChangedEventHandler PropertyChanged;

			public ObservableCollection<NoteItem> DisplayedData { get; } = new ObservableCollection<NoteItem>();

			public MainWindowData()
			{
				allViewItems = new List<NoteItem>();
			}

			public string DatesRange
			{
				get => datesRange;
				private set
				{
					datesRange = value;
					NotifyPropertyChanged("DatesRange");
				}
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

			private void InitDates(IEnumerable<INoteDisplayedData> newDisplay)
			{
				var res = allViewItems.Where(x => newDisplay.Contains(x.NoteData));

				DisplayedData.Clear();
				foreach (var item in res)
				{
					DisplayedData.Add(item);
				}
			}

			public void DisplayDates(IEnumerable<INoteDisplayedData> newDisplay, DateTime date)
			{
				DatesRange = date.ToString(DateFormat);
				InitDates(newDisplay);
			}

			public void DisplayDates(IEnumerable<INoteDisplayedData> newDisplay, DateTime first, DateTime last)
			{
				DatesRange = first.ToString(DateFormat) + "-" + last.ToString(DateFormat);
				InitDates(newDisplay);
			}

			private void NotifyPropertyChanged(string propName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
			}

			private const string DateFormat = "dd MMMM";
		}
	}
}
