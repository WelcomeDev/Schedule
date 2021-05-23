using Controller;
using Controller.DataApis;
using Schedule.GUIs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Schedule
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainController ctrl;
		private readonly NavigationService navigation;

		public MainWindow()
		{
			InitializeComponent();

			ctrl = new MainController(null);

			navigation = NavigationService.GetNavigationService(AddNoteFrame);

			//ассинхронное получение данных и продолжение в главном потоке
			ctrl.GetDisplayedDataAsync()
				.ContinueWith(t => InitSource(t.Result),
					TaskScheduler.Default);
		}

		private void InitSource(IEnumerable<INoteDisplayedData> result)
		{
			List<NoteItem> notes = new List<NoteItem>();
			foreach (var dat in result)
			{
				var note = new NoteItem(dat);
				note.MouseDoubleClick += Note_MouseDoubleClick;

				notes.Add(note);
			}

			notesListView.ItemsSource = notes;
		}

		private void Note_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var note = sender as NoteItem;

			DisplayNote(note.NoteData);
		}

		private void DisplayNote(INoteDisplayedData note)
		{
			navigation.Navigate(new NoteEditionPage(note));
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			var cal = sender as Calendar;

			var dates = cal.SelectedDates;
			var first = dates.First();
			var last = dates.Last();

			if (first.Equals(last))
			{
				ctrl.GetDisplayedDataAsync(first)
					.ContinueWith(t => InitSource(t.Result),
								TaskScheduler.Default);
			}
			else
			{
				ctrl.GetDisplayedDataAsync(first, last)
					.ContinueWith(t => InitSource(t.Result),
								TaskScheduler.Default);
			}

		}

		private void AddNewNoteButton_Click(object sender, RoutedEventArgs e)
		{
			var data = ctrl.AddNewNote();
			DisplayNote(data);
		}
	}
}
