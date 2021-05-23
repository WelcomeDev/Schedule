using Controller;
using Controller.DataApis;
using Schedule.GUIs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedule
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainController ctrl;

		public MainWindow()
		{
			InitializeComponent();

			ctrl = new MainController(null);

			//ассинхронное получение данных и продолжение в главном потоке
			ctrl.GetDisplayedDataAsync()
				.ContinueWith(t => InitSource(t.Result), 
					TaskScheduler.Default);
		}

		private void InitSource(IEnumerable<INoteDisplayedData> result)
		{
			List<NoteItem> notes = new List<NoteItem>();
			foreach(var dat in result)
			{
				var note = new NoteItem(dat);
				note.MouseDoubleClick += Note_MouseDoubleClick;

				notes.Add(note);
			}

			notesListView.ItemsSource = notes;
		}

		private void Note_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var o = sender as NoteItem;
			Debug.WriteLine($"Opening:...\n {o.NoteData}");
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}
	}
}
